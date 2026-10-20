using BBLift.Domain.Common;

namespace BBLift.Domain.Entities;

public class Routine : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public ICollection<RoutineExercise> RoutineExercises { get; set; } = [];
}