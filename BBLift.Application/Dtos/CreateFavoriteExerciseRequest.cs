namespace BBLift.Application.Dtos;

public record CreateFavoriteExerciseRequest(
    string UserId,
    Guid ExerciseId
);