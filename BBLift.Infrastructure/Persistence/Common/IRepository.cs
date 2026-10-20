using BBLift.Domain.Common;
using BBLift.Infrastructure.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BBLift.Infrastructure.Persistence.Common;

public interface IRepository<TEntity>
    : IQuery<TEntity>
    where TEntity : EntityBase
{
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken ct = default
    );

    Task<IEnumerable<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken ct = default
    );

    Task<Paginate<TEntity>> GetPagedAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 25,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken ct = default
    );

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken ct = default
    );

    Task<TEntity> AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> AddRangeAsync(ICollection<TEntity> entities);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);
    Task<TEntity> DeleteAsync(TEntity entity);
    Task<IEnumerable<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities);
}