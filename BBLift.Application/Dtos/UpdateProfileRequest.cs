namespace BBLift.Application.Dtos;

public record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string Email,
    double Weight,
    double Height
);