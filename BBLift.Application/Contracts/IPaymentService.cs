using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IPaymentService
{
    Task<Result<PaymentResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<PaymentResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<PaymentResponse>> CreateAsync(CreatePaymentRequest request, CancellationToken ct = default);
    Task<Result> UpdateStatusAsync(UpdatePaymentRequest request, CancellationToken ct = default);
}
