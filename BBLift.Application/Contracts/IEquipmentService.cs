using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IEquipmentService
{
    Task<Result<EquipmentResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<EquipmentResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<EquipmentResponse>> CreateAsync(CreateEquipmentRequest request, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateEquipmentRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}