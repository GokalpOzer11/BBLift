using BBLift.Domain.Enums;

namespace BBLift.Application.Dtos;

public record UpdatePaymentRequest(
    Guid PaymentId,
    PaymentStatus Status
);