using BBLift.Domain.Common;

namespace BBLift.Domain.Entities;

public class Cart : EntityBase
{
    public Guid AppUserId { get; set; }
    public AppUser? User { get; set; }
    
    public ICollection<CartItem> Items { get; set; } = [];
}