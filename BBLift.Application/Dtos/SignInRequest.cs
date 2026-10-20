namespace BBLift.Application.Dtos;

public record SignInRequest(
    string Email,
    string Password
);