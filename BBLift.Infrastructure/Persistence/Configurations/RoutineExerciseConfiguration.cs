using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class RoutineExerciseConfiguration
: EntityBaseConfiguration<RoutineExercise>
{
    protected override void ConfigureEntity(EntityTypeBuilder<RoutineExercise> builder)
    {
        builder.HasOne(re => re.Routine)
            .WithMany(r => r.RoutineExercises)
            .HasForeignKey(re => re.RoutineId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(re => re.Exercise)
            .WithMany()
            .HasForeignKey(re => re.ExerciseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}