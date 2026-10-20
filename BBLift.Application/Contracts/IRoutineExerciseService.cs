using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IRoutineExerciseService
{
    Task<Result<Guid>> CreateAsync(CreateRoutineExerciseRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, UpdateRoutineExerciseRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}