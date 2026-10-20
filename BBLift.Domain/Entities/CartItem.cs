using BBLift.Domain.Common;

namespace BBLift.Domain.Entities;

public class CartItem : EntityBase
{
    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }

    public DateOnly CreatedOn { get; set; }
    
    public Guid MembershipPackageId { get; set; }
    public MembershipPackage? Package { get; set; }
    
    public int Quantity { get; set; } = 1;
}