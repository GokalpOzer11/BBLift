using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Common;
using BBLift.Infrastructure.Persistence.Contracts;
using BBLift.Infrastructure.Persistence.Data;

namespace BBLift.Infrastructure.Persistence.Repositories;

public class CartRepository(AppDbContext context)
    : EfRepositoryBase<Cart, AppDbContext>(context),
        ICartRepository
{
}