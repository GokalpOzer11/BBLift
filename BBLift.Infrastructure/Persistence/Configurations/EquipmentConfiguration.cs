using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class EquipmentConfiguration 
    : EntityBaseConfiguration<Equipment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Equipment> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}