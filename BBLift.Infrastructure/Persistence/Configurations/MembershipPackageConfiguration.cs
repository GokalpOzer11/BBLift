using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class MembershipPackageConfiguration 
    : EntityBaseConfiguration<MembershipPackage>
{
    protected override void ConfigureEntity(EntityTypeBuilder<MembershipPackage> builder)
    {
        builder.OwnsOne(m => m.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.Amount).HasColumnName("Price_Amount").HasColumnType("decimal(18,2)").IsRequired();
            priceBuilder.Property(p => p.Currency).HasColumnName("Price_Currency").HasMaxLength(3).IsRequired();
            
            priceBuilder.HasData(
                new { MembershipPackageId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Amount = 299.90m, Currency = "TRY" }
            );
        });
        
        builder.HasData(
            new MembershipPackage
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Aylık Başlangıç Planı",
                Description = "BBLift'in tüm özelliklerine 1 ay erişim.",
                DurationInDays = 30,
                IsActive = true,
                CreatedOn = new DateTime(2026, 8, 30, 0, 0, 0, DateTimeKind.Utc) 
            }
        );
    }
}