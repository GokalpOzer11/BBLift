using BBLift.Domain.Common;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;

namespace BBLift.Domain.Entities;

public class Order : EntityBase
{
    public Guid AppUserId { get; set; }
    public AppUser? User { get; set; }
    
    public Guid MembershipPackageId { get; set; }
    public MembershipPackage? Package { get; set; }
    
    public Money TotalAmount { get; set; } = null!;
    public Payment? Payment { get; set; }
    
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}