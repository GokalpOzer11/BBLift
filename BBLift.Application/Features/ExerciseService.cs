using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;

namespace BBLift.Application.Features;

public class ExerciseService(
    IExerciseRepository repository,
    IValidator<CreateExerciseRequest> createValidator,
    IValidator<UpdateExerciseRequest> updateValidator
    ) : IExerciseService
{
    public async Task<Result<ExerciseResponse>> CreateAsync(CreateExerciseRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<ExerciseResponse>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var exercise = await repository.AddAsync(new Exercise
        {
            Name = request.Name,
            Description = request.Description,
            MuscleGroupId = request.MuscleGroupId,
            EquipmentId = request.EquipmentId
        });

        return Result<ExerciseResponse>.Success(new ExerciseResponse(
            exercise.Id,
            exercise.Name,
            exercise.Description,
            exercise.MuscleGroupId,
            exercise.EquipmentId
        ));
    }

    public async Task<Result> UpdateAsync(UpdateExerciseRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var exercise = await repository.GetAsync(predicate: x => x.Id == request.Id, ct: ct);
        if (exercise is null)
            return Result.Failure("Egzersiz bulunamadı.");

        exercise.Name = request.Name;
        exercise.Description = request.Description;
        exercise.MuscleGroupId = request.MuscleGroupId;
        exercise.EquipmentId = request.EquipmentId;

        await repository.UpdateAsync(exercise);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<ExerciseResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var exercises = await repository.GetListAsync(ct: ct);
        if (!exercises.Any())
            return Result<IEnumerable<ExerciseResponse>>.Failure("Sistemde kayıtlı egzersiz bulunmuyor.");

        return Result<IEnumerable<ExerciseResponse>>.Success(
            [.. exercises.Select(x => new ExerciseResponse(
                x.Id,
                x.Name,
                x.Description,
                x.MuscleGroupId,
                x.EquipmentId
            ))]
        );
    }

    public async Task<Result<ExerciseResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var exercise = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (exercise is null)
            return Result<ExerciseResponse>.Failure("Egzersiz bulunamadı.");

        return Result<ExerciseResponse>.Success(new ExerciseResponse(
            exercise.Id,
            exercise.Name,
            exercise.Description,
            exercise.MuscleGroupId,
            exercise.EquipmentId
        ));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var exercise = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (exercise is null)
            return Result.Failure("Egzersiz bulunamadı.");

        await repository.DeleteAsync(exercise);
        return Result.Success();
    }
}