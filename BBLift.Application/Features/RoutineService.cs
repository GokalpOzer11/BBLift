using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BBLift.Application.Features;

public class RoutineService(
    IRoutineRepository repository,
    IValidator<CreateRoutineRequest> createValidator,
    IValidator<UpdateRoutineRequest> updateValidator
    ) : IRoutineService
{
    private static RoutineExerciseResponse ToExerciseResponse(RoutineExercise re) => new(
        re.Id,
        re.ExerciseId,
        re.Exercise?.Name ?? string.Empty,
        re.Sets,
        (decimal)re.Weight.Value,
        re.Reps,
        re.RestTime is null
            ? (int?)null
            : re.RestTime.Hours * 3600 + re.RestTime.Minutes * 60 + re.RestTime.Seconds
    );

    private static RoutineResponse ToRoutineResponse(Routine r) => new(
        r.Id,
        r.Name,
        r.RoutineExercises
            .OrderBy(re => re.CreatedOn)
            .Select(re => ToExerciseResponse(re))
            .ToList()
    );

    public async Task<Result<RoutineResponse>> CreateAsync(CreateRoutineRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<RoutineResponse>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        Guid.TryParse(request.UserId, out var parsedUserId);

        var routine = await repository.AddAsync(new Routine
        {
            AppUserId = parsedUserId,
            Name = request.Name,
        });

        return Result<RoutineResponse>.Success(new RoutineResponse(
            routine.Id,
            routine.Name,
            []
        ));
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateRoutineRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var routine = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (routine is null)
            return Result.Failure("Antrenman programı bulunamadı.");

        routine.Name = request.Name;

        await repository.UpdateAsync(routine);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<RoutineResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var routines = await repository.GetListAsync(
            include: x => x.Include(r => r.RoutineExercises).ThenInclude(re => re.Exercise),
            ct: ct
        );

        if (!routines.Any())
            return Result<IEnumerable<RoutineResponse>>.Failure("Kayıtlı antrenman programı bulunmuyor.");

        return Result<IEnumerable<RoutineResponse>>.Success(
            [.. routines.Select(r => ToRoutineResponse(r))]
        );
    }

    public async Task<Result<RoutineResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var routine = await repository.GetAsync(
            predicate: x => x.Id == id,
            include: x => x.Include(r => r.RoutineExercises).ThenInclude(re => re.Exercise),
            ct: ct
        );

        if (routine is null)
            return Result<RoutineResponse>.Failure("Antrenman programı bulunamadı.");

        return Result<RoutineResponse>.Success(ToRoutineResponse(routine));
    }

    public async Task<Result<IEnumerable<RoutineResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        Guid.TryParse(userId, out var parsedUserId);

        var routines = await repository.GetListAsync(
            predicate: x => x.AppUserId == parsedUserId,
            include: x => x.Include(r => r.RoutineExercises).ThenInclude(re => re.Exercise),
            ct: ct
        );

        if (!routines.Any())
            return Result<IEnumerable<RoutineResponse>>.Failure("Bu kullanıcıya ait antrenman programı bulunamadı.");

        return Result<IEnumerable<RoutineResponse>>.Success(
            [.. routines.Select(r => ToRoutineResponse(r))]
        );
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var routine = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (routine is null)
            return Result.Failure("Antrenman programı bulunamadı.");

        await repository.DeleteAsync(routine);
        return Result.Success();
    }
}