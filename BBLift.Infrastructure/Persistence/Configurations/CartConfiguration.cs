using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class CartConfiguration
    : EntityBaseConfiguration<WorkoutSession>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.Property(x => x.AppUserId)
            .IsRequired();
    }
}