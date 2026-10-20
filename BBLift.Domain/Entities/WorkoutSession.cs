using BBLift.Domain.Common;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;

namespace BBLift.Domain.Entities;

public class WorkoutSession : EntityBase
{
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? EndTime { get; set; }

    public TimeDuration WorkOutDuration { get; set; }
    
    public Guid? RoutineId { get; set; }
    public string? Name { get; set; }
    
    public WorkoutStatus Status { get; set; } = WorkoutStatus.Active;
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
    
}