using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IWorkoutLogService
{
    Task<Result<IEnumerable<WorkoutLogResponse>>> GetBySessionIdAsync(Guid sessionId, CancellationToken ct = default);
    Task<Result> CreateAsync(CreateWorkoutLogRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateWorkoutLogRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}