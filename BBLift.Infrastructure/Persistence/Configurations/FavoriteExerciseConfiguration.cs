using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class FavoriteExerciseConfiguration
    : EntityBaseConfiguration<FavoriteExercise>
{
    protected override void ConfigureEntity(EntityTypeBuilder<FavoriteExercise> builder)
    {
        builder.HasOne(f => f.AppUser) 
            .WithMany() 
            .HasForeignKey(f => f.AppUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(f => f.Exercise)
            .WithMany() 
            .HasForeignKey(f => f.ExerciseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(f => new { f.AppUserId, f.ExerciseId })
            .IsUnique();
    }
}