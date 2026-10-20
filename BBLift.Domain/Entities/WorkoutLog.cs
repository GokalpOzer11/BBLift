using BBLift.Domain.Common;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;

namespace BBLift.Domain.Entities;

public class WorkoutLog : EntityBase
{
    public Guid WorkoutSessionId { get; set; }
    public WorkoutSession? WorkoutSession { get; set; }
    
    public Guid ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }

    public int SetNumber { get; set; }
    public Weight Weight { get; set; } = null;
    public int Reps { get; set; }
    public SetStatus Status { get; set; } = SetStatus.Normal; 
    
    public int? Rir { get; set; }

    public TimeDuration? RestTime { get; set; }
}