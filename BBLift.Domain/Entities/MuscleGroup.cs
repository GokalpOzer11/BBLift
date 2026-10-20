using BBLift.Domain.Common;

namespace BBLift.Domain.Entities;

public class MuscleGroup : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    
}