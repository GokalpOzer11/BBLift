namespace BBLift.Domain.Common;

public class EntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public bool IsActive { get; set; } = true;
    
    public DateTimeOffset CreatedOn { get; set; }
    
    public DateTimeOffset? ModifiedOn { get; set; }
    
    public DateTimeOffset? DeletedOn { get; set; }
}