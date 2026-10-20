namespace BBLift.Application.Dtos;

public record UpdateRoutineRequest
(
    string Name,
    Guid RoutineId
);