using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface ICartItemService
{
    Task<Result<IEnumerable<CartItemResponse>>> GetByCartIdAsync(Guid cartId, CancellationToken ct = default);
    Task<Result> CreateAsync(CreateCartItemRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateCartItemRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}