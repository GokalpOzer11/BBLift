using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Common;
using BBLift.Infrastructure.Persistence.Contracts;
using BBLift.Infrastructure.Persistence.Data;

namespace BBLift.Infrastructure.Persistence.Repositories;

public class MuscleGroupRepository(AppDbContext context)
    : EfRepositoryBase<MuscleGroup, AppDbContext>(context),
        IMuscleGroupRepository
{
}