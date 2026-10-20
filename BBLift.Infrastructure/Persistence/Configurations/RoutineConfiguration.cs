using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class RoutineConfiguration 
: EntityBaseConfiguration<Routine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Routine> builder)
    {
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.HasOne(r => r.AppUser) 
            .WithMany() 
            .HasForeignKey(r => r.AppUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}