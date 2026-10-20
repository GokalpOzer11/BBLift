using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class CartItemConfiguration 
    :EntityBaseConfiguration<CartItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CartItem> builder)
    {
        builder.Property(x => x.Quantity)
            .IsRequired();
        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId);
        builder.HasOne(ci => ci.Package)
            .WithMany() 
            .HasForeignKey(ci => ci.MembershipPackageId);
    }
}