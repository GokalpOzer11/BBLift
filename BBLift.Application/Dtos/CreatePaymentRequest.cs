using BBLift.Domain.Enums;

namespace BBLift.Application.Dtos;

public record CreatePaymentRequest
(
    Guid OrderId,
    decimal Amount,
    PaymentMethod Method
);