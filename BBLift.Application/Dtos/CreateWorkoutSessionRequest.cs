using BBLift.Domain.Enums;

namespace BBLift.Application.Dtos;

public record CreateWorkoutSessionRequest(
    string UserId,
    Guid? RoutineId,
    WorkoutStatus Status
);