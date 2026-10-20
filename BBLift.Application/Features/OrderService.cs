using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Domain.ValueObjects;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BBLift.Application.Features;

public class OrderService(
    IOrderRepository repository,
    ICartService cartService,
    IValidator<CreateOrderRequest> createValidator,
    IValidator<UpdateOrderRequest> updateStatusValidator
    ) : IOrderService
{
    public async Task<Result<OrderResponse>> CreateAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<OrderResponse>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        Guid.TryParse(request.UserId, out var parsedUserId);

        var order = new Order
        {
            AppUserId = parsedUserId,
            MembershipPackageId = request.MembershipPackageId,
            TotalAmount = Money.TRY(request.TotalAmount),
            Status = BBLift.Domain.Enums.OrderStatus.Pending
        };

        var createdOrder = await repository.AddAsync(order);

        await cartService.ClearAsync(request.UserId, ct);

        return Result<OrderResponse>.Success(new OrderResponse(
            createdOrder.Id,
            createdOrder.AppUserId,
            createdOrder.MembershipPackageId,
            createdOrder.Package?.Name ?? "Abonelik Paketi",
            createdOrder.TotalAmount.Amount,
            createdOrder.TotalAmount.Currency,
            createdOrder.Status,
            createdOrder.Payment?.Id
        ));
    }

    public async Task<Result> UpdateStatusAsync(UpdateOrderRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateStatusValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var order = await repository.GetAsync(predicate: x => x.Id == request.Id, ct: ct);
        if (order is null)
            return Result.Failure("Sipariş bulunamadı.");

        order.Status = request.Status;

        await repository.UpdateAsync(order);

        return Result.Success();
    }

    public async Task<Result<OrderResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var order = await repository.GetAsync(
            predicate: x => x.Id == id,
            include: x => x.Include(o => o.Package).Include(o => o.Payment),
            ct: ct
        );

        if (order is null)
            return Result<OrderResponse>.Failure("Sipariş bulunamadı.");

        return Result<OrderResponse>.Success(new OrderResponse(
            order.Id,
            order.AppUserId,
            order.MembershipPackageId,
            order.Package?.Name ?? "Bilinmeyen Paket",
            order.TotalAmount.Amount,
            order.TotalAmount.Currency,
            order.Status,
            order.Payment?.Id
        ));
    }

    public async Task<Result<IEnumerable<OrderResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        Guid.TryParse(userId, out var parsedUserId);

        var orders = await repository.GetListAsync(
            predicate: x => x.AppUserId == parsedUserId,
            include: x => x.Include(o => o.Package).Include(o => o.Payment),
            ct: ct
        );

        if (!orders.Any())
            return Result<IEnumerable<OrderResponse>>.Failure("Bu kullanıcıya ait sipariş bulunmuyor.");

        return Result<IEnumerable<OrderResponse>>.Success(
            [.. orders.Select(o => new OrderResponse(
                o.Id,
                o.AppUserId,
                o.MembershipPackageId,
                o.Package?.Name ?? "Bilinmeyen Paket",
                o.TotalAmount.Amount,
                o.TotalAmount.Currency,
                o.Status,
                o.Payment?.Id
            ))]
        );
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var order = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (order is null)
            return Result.Failure("Sipariş bulunamadı.");

        await repository.DeleteAsync(order);
        return Result.Success();
    }
}