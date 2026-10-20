using BBLift.Domain.Entities;
using BBLift.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration
    : EntityBaseConfiguration<Payment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Payment> builder)
    {
        builder.OwnsOne(x => x.Amount, m =>
        {
            m.Property(x => x.Amount)
                .HasColumnName("Amount")
                .HasColumnType("money");
            m.Property(x => x.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3);
        });
        builder.Property(x => x.Method)
            .HasConversion(
                x => x.ToString(),
                x => Enum.Parse<PaymentMethod>(x)
            );
        builder.Property(x => x.Status)
            .HasConversion(
                x => x.ToString(),
                x => Enum.Parse<PaymentStatus>(x)
            );
        builder.HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderId);
    }
}