namespace BBLift.Application.Dtos;

public record MuscleGroupResponse(
    Guid Id, 
    string Name, 
    string? GifUrl
);