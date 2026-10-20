using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration
    : EntityBaseConfiguration<Exercise>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.HasOne(e => e.MuscleGroup)
            .WithMany() 
            .HasForeignKey(e => e.MuscleGroupId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.Equipment)
            .WithMany() 
            .HasForeignKey(e => e.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}