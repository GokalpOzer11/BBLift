using BBLift.Domain.Common;

namespace BBLift.Domain.Entities;

public class Equipment : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Exercise> Exercises { get; set; }  = new List<Exercise>();
}