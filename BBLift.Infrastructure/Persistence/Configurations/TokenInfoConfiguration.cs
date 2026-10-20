using BBLift.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public class TokenInfoConfiguration
: EntityBaseConfiguration<TokenInfo>
{
    protected override void ConfigureEntity(EntityTypeBuilder<TokenInfo> builder)
    {
        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.ExpiredAt)
            .IsRequired();
    }
}