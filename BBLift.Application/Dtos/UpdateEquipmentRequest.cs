namespace BBLift.Application.Dtos;

public record UpdateEquipmentRequest(
    Guid Id,
    string Name
);