using BBLift.Domain.Common;

namespace BBLift.Domain.Entities;

public class FavoriteExercise : EntityBase
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    
}