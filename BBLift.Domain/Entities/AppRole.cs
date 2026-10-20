using Microsoft.AspNetCore.Identity;

namespace BBLift.Domain.Entities;

public class AppRole
    : IdentityRole<Guid>
{
    public DateTimeOffset CreatedAt { get; set; }
}