using BBLift.Domain.Enums;

namespace BBLift.Application.Dtos;

public record WorkoutSessionResponse
(
    Guid Id,
    Guid? RoutineId, 
    string? Name,    
    DateTimeOffset StartTime,
    DateTimeOffset? EndTime,
    int WorkOutDurationInSeconds,
    WorkoutStatus Status
);