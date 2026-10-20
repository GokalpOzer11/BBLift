using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace BBLift.Domain.Entities;

public class AppUser 
    : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    
    public DateTimeOffset CreatedOn { get; set; }
    
    public bool IsActive { get; set; }
    
    public Weight Weight { get; set; } = Weight.Create(1,WeightUnit.Kg);
    public Height Height { get; set; } = Height.Create(1,HeightUnit.Cm);
    
    public ICollection<FavoriteExercise> FavoriteExercises { get; set; } = new List<FavoriteExercise>();
    
    public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
    
    public WeightUnit PreferredWeightUnit { get; set; } = WeightUnit.Kg;
    
}
