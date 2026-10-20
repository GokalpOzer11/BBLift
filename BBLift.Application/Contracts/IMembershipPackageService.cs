using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IMembershipPackageService
{
    Task<Result<MembershipPackageResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<MembershipPackageResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<MembershipPackageResponse>> CreateAsync(CreateMembershipPackageRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateMembershipPackageRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}