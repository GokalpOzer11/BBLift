using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IUserManagementService
{
    Task<Result<IEnumerable<UserResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> AddToRoleAsync(Guid userId, string role, CancellationToken ct = default);
    Task<Result> RemoveFromRoleAsync(Guid userId, string role, Guid currentUserId, CancellationToken ct = default);
}