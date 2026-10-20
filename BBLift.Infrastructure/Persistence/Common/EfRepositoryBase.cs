using BBLift.Domain.Common;
using BBLift.Infrastructure.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BBLift.Infrastructure.Persistence.Common;

public class EfRepositoryBase<TEntity, TContext>(TContext context)
    : IRepository<TEntity>
    where TEntity : EntityBase
    where TContext : DbContext
{
    public IQueryable<TEntity> Query() => context.Set<TEntity>();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.Id = Guid.CreateVersion7(TimeProvider.System.GetUtcNow());
        entity.CreatedOn = TimeProvider.System.GetUtcNow();
        entity.IsActive = true;
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.Id = Guid.CreateVersion7(TimeProvider.System.GetUtcNow());
            entity.CreatedOn = TimeProvider.System.GetUtcNow();
            entity.IsActive = true;
        }
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
        return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.ModifiedOn = TimeProvider.System.GetUtcNow();
        context.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.ModifiedOn = TimeProvider.System.GetUtcNow();
        context.UpdateRange(entities);
        await context.SaveChangesAsync();
        return entities;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        entity.DeletedOn = TimeProvider.System.GetUtcNow();
        entity.IsActive = false;
        context.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.DeletedOn = TimeProvider.System.GetUtcNow();
            entity.IsActive = false;
        }
        context.UpdateRange(entities);
        await context.SaveChangesAsync();
        return entities;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken ct = default
    )
    {
        IQueryable<TEntity> query = Query();
        if (!enableTracking)
            query = query.AsNoTracking();
        if (withDeleted)
            query = query.IgnoreQueryFilters();
        if (predicate is not null)
            query = query.Where(predicate);
        return await query.AnyAsync(ct);
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool withDeleted = false, 
        bool enableTracking = false, 
        CancellationToken ct = default
    )
    {
        IQueryable<TEntity> query = Query();
        if (!enableTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        if (withDeleted)
            query = query.IgnoreQueryFilters();
        return await query.SingleOrDefaultAsync(predicate, ct);
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool withDeleted = false, 
        bool enableTracking = false, 
        CancellationToken ct = default
    )
    {
        IQueryable<TEntity> query = Query();
        if (!enableTracking)
            query = query.AsNoTracking();
        if (predicate is not null)
            query = query.Where(predicate);
        if (include is not null)
            query = include(query);
        if (withDeleted)
            query = query.IgnoreQueryFilters();
        if (orderBy is not null)
            return await orderBy(query).ToListAsync(ct);
        return await query.ToListAsync(ct);
    }

    public async Task<Paginate<TEntity>> GetPagedAsync(
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        int index = 0, 
        int size = 25, 
        bool withDeleted = false, 
        bool enableTracking = false, 
        CancellationToken ct = default
    )
    {
        IQueryable<TEntity> query = Query();
        if (!enableTracking)
            query = query.AsNoTracking();
        if (predicate is not null)
            query = query.Where(predicate);
        if (include is not null)
            query = include(query);
        if (withDeleted)
            query = query.IgnoreQueryFilters();
        if (orderBy is not null)
            return await orderBy(query).ToPaginateAsync(index, size, ct);
        return await query.ToPaginateAsync(index, size, ct);
    }
}