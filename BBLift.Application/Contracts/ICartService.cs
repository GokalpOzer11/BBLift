using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface ICartService
{
    Task<Result<CartResponse>> CreateAsync(CreateCartRequest request, CancellationToken cancellationToken = default);
    Task<Result> AddItemAsync(CreateCartItemRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateItemAsync(UpdateCartItemRequest request, CancellationToken cancellationToken = default);
    Task<Result> RemoveItemAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> ClearAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<CartResponse>> GetByUserIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<CartResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}