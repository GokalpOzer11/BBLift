using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IFavoriteExerciseService
{
    Task<Result<IEnumerable<FavoriteExerciseResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Result> CreateAsync(CreateFavoriteExerciseRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}