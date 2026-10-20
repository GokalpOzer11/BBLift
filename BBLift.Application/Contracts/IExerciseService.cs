using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IExerciseService
{
    Task<Result<ExerciseResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<ExerciseResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<ExerciseResponse>> CreateAsync(CreateExerciseRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateExerciseRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}