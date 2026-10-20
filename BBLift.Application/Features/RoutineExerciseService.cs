using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;

namespace BBLift.Application.Features;

public class RoutineExerciseService(
    IRoutineExerciseRepository repository,
    IValidator<CreateRoutineExerciseRequest> createValidator,
    IValidator<UpdateRoutineExerciseRequest> updateValidator
    ) : IRoutineExerciseService
{
    public async Task<Result<Guid>> CreateAsync(CreateRoutineExerciseRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<Guid>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var routineExercise = new RoutineExercise
        {
            RoutineId = request.RoutineId,
            ExerciseId = request.ExerciseId,
            Sets = request.Sets,
            Reps = request.Reps,
            Weight = Weight.Create((double)request.Weight, WeightUnit.Kg),
            RestTime = request.RestTimeInSeconds.HasValue
                ? TimeDuration.Create(0, 0, request.RestTimeInSeconds.Value)
                : null
        };

        var created = await repository.AddAsync(routineExercise);

        return Result<Guid>.Success(created.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateRoutineExerciseRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var routineExercise = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);

        if (routineExercise is null)
            return Result.Failure("Antrenman programında böyle bir egzersiz bulunamadı.");

        if (routineExercise.RoutineId != request.RoutineId)
            return Result.Failure("Bu egzersiz belirtilen rutine ait değil.");

        routineExercise.Sets = request.Sets;
        routineExercise.Reps = request.Reps;
        routineExercise.Weight = Weight.Create((double)request.Weight, WeightUnit.Kg);
        routineExercise.RestTime = request.RestTimeInSeconds.HasValue
            ? TimeDuration.Create(0, 0, request.RestTimeInSeconds.Value)
            : null;

        await repository.UpdateAsync(routineExercise);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var routineExercise = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (routineExercise is null)
            return Result.Failure("Antrenman programından silinecek egzersiz bulunamadı.");

        await repository.DeleteAsync(routineExercise);
        return Result.Success();
    }
}