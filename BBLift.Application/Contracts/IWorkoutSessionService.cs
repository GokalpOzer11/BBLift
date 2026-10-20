using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IWorkoutSessionService
{
    Task<Result<WorkoutSessionResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<WorkoutSessionResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Result<WorkoutSessionResponse>> CreateAsync(CreateWorkoutSessionRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateWorkoutSessionRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}