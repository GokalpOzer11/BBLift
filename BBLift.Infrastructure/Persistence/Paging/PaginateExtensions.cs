using Microsoft.EntityFrameworkCore;

namespace BBLift.Infrastructure.Persistence.Paging;

public static class PaginateExtensions
{
    public static async Task<Paginate<TEntity>> ToPaginateAsync<TEntity>(
        this IQueryable<TEntity> source,
        int index,
        int size,
        CancellationToken ct = default
    )
    {
        int count = await source.CountAsync(ct);
        var items = await source
            .Skip(index * size)
            .Take(size)
            .ToListAsync(ct);
        Paginate<TEntity> list = new() 
        {
            Index = index,
            Size = size,
            Count = count,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size)
        };
        return list;
    }
}
