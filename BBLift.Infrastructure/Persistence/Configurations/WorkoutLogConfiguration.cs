using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class WorkoutLogConfiguration 
    :EntityBaseConfiguration<WorkoutLog>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WorkoutLog> builder)
    {
        builder.OwnsOne(w => w.Weight, weightBuilder =>
        {
            weightBuilder.Property(w => w.Value)
                .HasColumnName("Weight_Value")
                .IsRequired();

            weightBuilder.Property(w => w.Unit)
                .HasColumnName("Weight_Unit")
                .IsRequired();
        });
        
        builder.OwnsOne(w => w.RestTime, timeBuilder =>
        {
            timeBuilder.Property(t => t.Hours).HasColumnName("RestTime_Hours");
            timeBuilder.Property(t => t.Minutes).HasColumnName("RestTime_Minutes");
            timeBuilder.Property(t => t.Seconds).HasColumnName("RestTime_Seconds");
        });
        
        builder.Property(w => w.Rir).IsRequired(false);
    }
}