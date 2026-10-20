using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IMuscleGroupService
{
    Task<Result<MuscleGroupResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<MuscleGroupResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<MuscleGroupResponse>> CreateAsync(CreateMuscleGroupRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateMuscleGroupRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}