using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class WorkoutSessionConfiguration
: EntityBaseConfiguration<WorkoutSession>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.HasOne(ws => ws.AppUser) 
            .WithMany()
            .HasForeignKey(ws => ws.AppUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<AppUser>() 
            .WithMany()
            .HasForeignKey(ws => ws.AppUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}