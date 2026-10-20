using BBLift.Domain.Common;
using BBLift.Domain.ValueObjects;

namespace BBLift.Domain.Entities;

public class MembershipPackage : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Money Price { get; set; } = null!;
    
    public int DurationInDays { get; set; }
    public bool IsActive { get; set; } = true;
}   