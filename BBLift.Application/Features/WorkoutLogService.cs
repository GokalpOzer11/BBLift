using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BBLift.Application.Features;

public class WorkoutLogService(
    IWorkoutLogRepository repository,
    IValidator<CreateWorkoutLogRequest> createValidator,
    IValidator<UpdateWorkoutLogRequest> updateValidator
    ) : IWorkoutLogService
{
    public async Task<Result> CreateAsync(CreateWorkoutLogRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var log = new WorkoutLog
        {
            WorkoutSessionId = request.WorkoutSessionId,
            ExerciseId = request.ExerciseId,
            SetNumber = request.SetNumber,
            Reps = request.Reps,
            Status = request.Status,
            Rir = request.Rir,
            Weight = Weight.Create((double)request.Weight, WeightUnit.Kg),
            RestTime = request.RestTimeInSeconds.HasValue 
                ? TimeDuration.Create(0, 0, request.RestTimeInSeconds.Value)
                : null
        };

        await repository.AddAsync(log);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(UpdateWorkoutLogRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var log = await repository.GetAsync(predicate: x => x.Id == request.Id, ct: ct);
        if (log is null)
            return Result.Failure("Set kaydı bulunamadı.");

        log.Reps = request.Reps;
        log.Status = request.Status;
        log.Rir = request.Rir;
        log.Weight = Weight.Create((double)request.Weight, WeightUnit.Kg);
        log.RestTime = request.RestTimeInSeconds.HasValue 
            ? TimeDuration.Create(0, 0, request.RestTimeInSeconds.Value)
            : null;

        await repository.UpdateAsync(log);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<WorkoutLogResponse>>> GetBySessionIdAsync(Guid sessionId, CancellationToken ct = default)
    {
        var logs = await repository.GetListAsync(
            predicate: x => x.WorkoutSessionId == sessionId, 
            include: query => query.Include(l => l.Exercise), 
            ct: ct);

        if (!logs.Any())
            return Result<IEnumerable<WorkoutLogResponse>>.Failure("Bu oturuma ait set kaydı bulunamadı.");

        return Result<IEnumerable<WorkoutLogResponse>>.Success(
            [.. logs.Select(x => new WorkoutLogResponse(
                x.Id,
                x.WorkoutSessionId,
                x.ExerciseId,
                x.Exercise?.Name ?? "Bilinmeyen Egzersiz", 
                x.SetNumber,
                (decimal)(x.Weight?.Value ?? 0), 
                x.Reps,
                x.Status,
                x.Rir,
                x.RestTime?.Seconds 
            ))]
        );
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var log = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (log is null)
            return Result.Failure("Set kaydı bulunamadı.");

        await repository.DeleteAsync(log);
        return Result.Success();
    }
}