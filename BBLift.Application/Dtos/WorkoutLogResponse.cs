using BBLift.Domain.Enums;

namespace BBLift.Application.Dtos;

public record WorkoutLogResponse
(
    Guid Id,
    Guid WorkoutSessionId,
    Guid ExerciseId,
    string ExerciseName,
    int SetNumber,
    decimal Weight,
    int Reps,
    SetStatus Status,
    int? Rir,
    int? RestTimeInSeconds
);