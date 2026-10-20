using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;

namespace BBLift.Application.Features;

public class WorkoutSessionService(
    IWorkoutSessionRepository repository,
    IValidator<CreateWorkoutSessionRequest> createValidator,
    IValidator<UpdateWorkoutSessionRequest> updateValidator
    ) : IWorkoutSessionService
{
    public async Task<Result<WorkoutSessionResponse>> CreateAsync(CreateWorkoutSessionRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<WorkoutSessionResponse>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        Guid.TryParse(request.UserId, out var parsedUserId);

        var session = new WorkoutSession
        {
            AppUserId = parsedUserId,
            RoutineId = request.RoutineId, 
            Name = null,                  
            StartTime = DateTimeOffset.UtcNow,
            Status = WorkoutStatus.Active,
            WorkOutDuration = TimeDuration.Create(0, 0, 0) 
        };

        var createdSession = await repository.AddAsync(session);

        return Result<WorkoutSessionResponse>.Success(new WorkoutSessionResponse(
            createdSession.Id,
            createdSession.RoutineId, 
            createdSession.Name,      
            createdSession.StartTime,
            createdSession.EndTime,
            createdSession.WorkOutDuration.Seconds, 
            createdSession.Status
        ));
    }

    public async Task<Result> UpdateAsync(UpdateWorkoutSessionRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var session = await repository.GetAsync(predicate: x => x.Id == request.Id, ct: ct);
        if (session is null)
            return Result.Failure("Antrenman oturumu bulunamadı.");

        
        if (request.Name != null)
        {
            session.Name = request.Name;
        }

        session.EndTime = request.EndTime;
        session.Status = request.Status;
        session.WorkOutDuration = TimeDuration.Create(0, 0, request.WorkOutDurationInSeconds);

        await repository.UpdateAsync(session);

        return Result.Success();
    }

    public async Task<Result<WorkoutSessionResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var session = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (session is null)
            return Result<WorkoutSessionResponse>.Failure("Antrenman oturumu bulunamadı.");

        return Result<WorkoutSessionResponse>.Success(new WorkoutSessionResponse(
            session.Id,
            session.RoutineId, 
            session.Name,      
            session.StartTime,
            session.EndTime,
            session.WorkOutDuration.Seconds, 
            session.Status
        ));
    }

    public async Task<Result<IEnumerable<WorkoutSessionResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        Guid.TryParse(userId, out var parsedUserId);
        
        var sessions = await repository.GetListAsync(predicate: x => x.AppUserId == parsedUserId, ct: ct);
        if (!sessions.Any())
            return Result<IEnumerable<WorkoutSessionResponse>>.Failure("Bu kullanıcıya ait antrenman bulunmuyor.");

        return Result<IEnumerable<WorkoutSessionResponse>>.Success(
            [.. sessions.Select(session => new WorkoutSessionResponse(
                session.Id,
                session.RoutineId, 
                session.Name,      
                session.StartTime,
                session.EndTime,
                session.WorkOutDuration.Seconds, 
                session.Status
            ))]
        );
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var session = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (session is null)
            return Result.Failure("Antrenman oturumu bulunamadı.");

        await repository.DeleteAsync(session);
        return Result.Success();
    }
}