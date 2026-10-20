using BBLift.Domain.Common;
using BBLift.Domain.ValueObjects;

namespace BBLift.Domain.Entities;

public class RoutineExercise : EntityBase
{
    public Guid RoutineId { get; set; }
    public Routine? Routine { get; set; }
    
    public Guid ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }

    public int Sets { get; set; }
    public Weight Weight { get; set; }
    public int Reps { get; set; }

    public TimeDuration? RestTime { get; set; }
}