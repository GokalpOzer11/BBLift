namespace BBLift.Infrastructure.Persistence.Common;

public interface IQuery<TEntity>
{
    IQueryable<TEntity> Query();
}