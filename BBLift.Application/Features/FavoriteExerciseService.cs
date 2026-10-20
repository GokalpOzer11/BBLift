using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BBLift.Application.Features;

public class FavoriteExerciseService(
    IFavoriteExerciseRepository repository,
    IValidator<CreateFavoriteExerciseRequest> createValidator
    ) : IFavoriteExerciseService
{
    public async Task<Result> CreateAsync(CreateFavoriteExerciseRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        Guid.TryParse(request.UserId, out var parsedUserId);

        var exists = await repository.GetAsync(
            predicate: x => x.AppUserId == parsedUserId && x.ExerciseId == request.ExerciseId, ct: ct);
            
        if (exists is not null)
            return Result.Failure("Bu egzersiz zaten favorilerinizde ekli.");

        var favorite = new FavoriteExercise
        {
            AppUserId = parsedUserId,
            ExerciseId = request.ExerciseId
        };

        await repository.AddAsync(favorite);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<FavoriteExerciseResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        Guid.TryParse(userId, out var parsedUserId);

        var favorites = await repository.GetListAsync(
            predicate: x => x.AppUserId == parsedUserId,
            include: x => x.Include(f => f.Exercise),
            ct: ct
        );

        if (!favorites.Any())
            return Result<IEnumerable<FavoriteExerciseResponse>>.Failure("Favori egzersiz bulunamadı.");

        return Result<IEnumerable<FavoriteExerciseResponse>>.Success(
            [.. favorites.Select(f => new FavoriteExerciseResponse(
                f.Id,
                f.ExerciseId,
                f.Exercise!.Name 
            ))]
        );
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var favorite = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (favorite is null)
            return Result.Failure("Favori egzersiz kaydı bulunamadı.");

        await repository.DeleteAsync(favorite);
        return Result.Success();
    }
}