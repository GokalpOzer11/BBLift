namespace BBLift.Application.Dtos;

public record UserProfileResponse(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    DateOnly DateOfBirth,
    double Weight,
    double Height,
    int TotalWorkouts, 
    decimal TotalVolume, 
    List<RecentWorkoutResponse> RecentWorkouts
);