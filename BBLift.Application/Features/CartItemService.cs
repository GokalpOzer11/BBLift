using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BBLift.Application.Features;

public class CartItemService(
    ICartItemRepository repository,
    IValidator<CreateCartItemRequest> createValidator,
    IValidator<UpdateCartItemRequest> updateValidator
    ) : ICartItemService
{
    public async Task<Result> CreateAsync(CreateCartItemRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var cartItem = new CartItem
        {
            CartId = request.CartId,
            MembershipPackageId = request.MembershipPackageId,
            Quantity = request.Quantity,
            CreatedOn = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await repository.AddAsync(cartItem);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(UpdateCartItemRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var cartItem = await repository.GetAsync(predicate: x => x.Id == request.Id, ct: ct);
        if (cartItem is null)
            return Result.Failure("Sepet ürünü bulunamadı.");

        cartItem.Quantity = request.Quantity;

        await repository.UpdateAsync(cartItem);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<CartItemResponse>>> GetByCartIdAsync(Guid cartId, CancellationToken ct = default)
    {
        var cartItems = await repository.GetListAsync(
            predicate: x => x.CartId == cartId,
            include: x => x.Include(c => c.Package), 
            ct: ct
        );

        if (!cartItems.Any())
            return Result<IEnumerable<CartItemResponse>>.Failure("Sepette ürün bulunmuyor.");

        return Result<IEnumerable<CartItemResponse>>.Success(
            [.. cartItems.Select(c => new CartItemResponse(
                c.Id,
                c.CartId,
                c.MembershipPackageId,
                c.Package!.Name, 
                c.Quantity,
                c.CreatedOn
            ))]
        );
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var cartItem = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (cartItem is null)
            return Result.Failure("Sepet ürünü bulunamadı.");

        await repository.DeleteAsync(cartItem);
        return Result.Success();
    }
}