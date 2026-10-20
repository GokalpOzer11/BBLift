using BBLift.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBLift.Infrastructure.Persistence.Configurations;

public abstract class EntityBaseConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedOn)
            .IsRequired();
        builder.Property(x => x.ModifiedOn)
            .IsRequired(false);
        builder.Property(x => x.DeletedOn)
            .IsRequired(false);
        builder.Property(x => x.IsActive)
            .IsRequired();
        builder.HasQueryFilter(x => x.IsActive);
        builder.HasIndex(x => x.CreatedOn);
        builder.HasIndex(x => x.IsActive)
            .HasFilter("[IsActive] = 1");
        ConfigureEntity(builder);
    }
    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}