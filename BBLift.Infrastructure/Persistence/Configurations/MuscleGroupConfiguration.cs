using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class MuscleGroupConfiguration 
    : EntityBaseConfiguration<MuscleGroup>
{
    protected override void ConfigureEntity(EntityTypeBuilder<MuscleGroup> builder)
    {
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}