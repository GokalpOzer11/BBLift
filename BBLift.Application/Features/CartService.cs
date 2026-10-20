using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BBLift.Application.Features;

public class CartService(
    ICartRepository cartRepository,
    ICartItemRepository cartItemRepository,
    IValidator<CreateCartRequest> cartCreateValidator,
    IValidator<CreateCartItemRequest> cartItemCreateValidator,
    IValidator<UpdateCartItemRequest> updateCartItemValidator
    )
    : ICartService
{
    public async Task<Result<CartResponse>> CreateAsync(
        CreateCartRequest request,
        CancellationToken cancellationToken = default
        )
    {
        var validationResult = await cartCreateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<CartResponse>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        
        var userGuid = Guid.Parse(request.UserId);

        var cart = await cartRepository.AddAsync(new Cart
        {
            AppUserId = userGuid
        });

        var item = new CartItem
        {
            CartId = cart.Id,
            MembershipPackageId = request.MembershipPackageId,
            Quantity = request.Quantity,
            CreatedOn = DateOnly.FromDateTime(DateTime.UtcNow) 
        };
        await cartItemRepository.AddAsync(item);

        return Result<CartResponse>.Success(new CartResponse(
            cart.Id,
            cart.AppUserId, 
            [new CartItemResponse(
                item.Id,
                item.CartId,
                item.MembershipPackageId,
                "Abonelik Paketi",
                item.Quantity,
                item.CreatedOn 
            )]
        ));
    }

    public async Task<Result> AddItemAsync(
        CreateCartItemRequest request,
        CancellationToken cancellationToken = default
        )
    {
        var validationResult = await cartItemCreateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        
        var cart = await cartRepository.GetAsync(predicate: x => x.Id == request.CartId, ct: cancellationToken);
        if (cart is null)
            return Result.Failure("Sepet bulunamadı.");

        await cartItemRepository.AddAsync(new CartItem
        {
            CartId = request.CartId,
            MembershipPackageId = request.MembershipPackageId, 
            Quantity = request.Quantity,
            CreatedOn = DateOnly.FromDateTime(DateTime.UtcNow) 
        });

        return Result.Success();
    }

    public async Task<Result> UpdateItemAsync(
        UpdateCartItemRequest request,
        CancellationToken cancellationToken = default
        )
    {
        var validationResult = await updateCartItemValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        
        var item = await cartItemRepository.GetAsync(predicate: x => x.Id == request.Id, ct: cancellationToken);
        if (item is null)
            return Result.Failure("Sepet ürünü bulunamadı.");

        item.Quantity = request.Quantity;
        await cartItemRepository.UpdateAsync(item);

        return Result.Success();
    }

    public async Task<Result> RemoveItemAsync(
        Guid id, 
        CancellationToken cancellationToken = default
        )
    {
        var item = await cartItemRepository.GetAsync(predicate: x => x.Id == id, ct: cancellationToken);
        if (item is null)
            return Result.Failure("Sepet ürünü bulunamadı.");

        await cartItemRepository.DeleteAsync(item); 
        
        return Result.Success();
    }

    public async Task<Result> ClearAsync(string userId, CancellationToken cancellationToken = default)
    {
        var userGuid = Guid.Parse(userId); 

        var cart = await cartRepository.GetAsync(predicate: x => x.AppUserId == userGuid, ct: cancellationToken);
        if (cart is null)
            return Result.Failure("Sepet bulunamadı.");

        await cartRepository.DeleteAsync(cart);
        
        return Result.Success();
    }

    public async Task<Result<CartResponse>> GetByUserIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var userGuid = Guid.Parse(id); 

        var cart = await cartRepository.GetAsync(
            predicate: x => x.AppUserId == userGuid,
            include: x => x.Include(c => c.Items).ThenInclude(i => i.Package),
            ct: cancellationToken
            );
            
        if (cart is null)
            return Result<CartResponse>.Failure("Sepet bulunamadı.");

        return Result<CartResponse>.Success(new CartResponse(
            cart.Id,
            cart.AppUserId, 
            cart.Items.Select(i => new CartItemResponse( 
                i.Id,
                i.CartId,
                i.MembershipPackageId,
                i.Package?.Name ?? "Abonelik Paketi", 
                i.Quantity,
                i.CreatedOn
            ))
        ));
    }

    public async Task<Result<CartResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetAsync(
            predicate: x => x.Id == id,
            include: x => x.Include(c => c.Items).ThenInclude(i => i.Package),
            ct: cancellationToken
            );
            
        if (cart is null)
            return Result<CartResponse>.Failure("Sepet bulunamadı.");

        return Result<CartResponse>.Success(new CartResponse(
            cart.Id,
            cart.AppUserId, 
            cart.Items.Select(i => new CartItemResponse(
                i.Id,
                i.CartId,
                i.MembershipPackageId,
                i.Package?.Name ?? "Abonelik Paketi", 
                i.Quantity,
                i.CreatedOn
            ))
        ));
    }
}