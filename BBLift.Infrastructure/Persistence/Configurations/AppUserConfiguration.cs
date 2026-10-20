using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace BBLift.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration 
    : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.OwnsOne(u => u.Height, heightBuilder =>
        {
            heightBuilder.Property(h => h.Value)
                .HasColumnName("Height_Value")
                .IsRequired(false);

            heightBuilder.Property(h => h.Unit)
                .HasColumnName("Height_Unit")
                .HasMaxLength(10)
                .IsRequired(false);
        });
    }
}