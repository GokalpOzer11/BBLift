using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class OrderConfiguration
    : EntityBaseConfiguration<Order>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.TotalAmount, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount)
                .HasColumnName("TotalAmount_Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            moneyBuilder.Property(m => m.Currency)
                .HasColumnName("TotalAmount_Currency")
                .HasMaxLength(3)
                .IsRequired();
        });
    }
}