using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IRoutineService
{
    Task<Result<RoutineResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<RoutineResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<RoutineResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Result<RoutineResponse>> CreateAsync(CreateRoutineRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, UpdateRoutineRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}