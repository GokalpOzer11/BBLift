using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IOrderService
{
    Task<Result<OrderResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<OrderResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Result<OrderResponse>> CreateAsync(CreateOrderRequest request, CancellationToken ct = default);
    Task<Result> UpdateStatusAsync(UpdateOrderRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}