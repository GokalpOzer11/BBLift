using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;

namespace BBLift.Application.Dtos;

public record PaymentResponse
(
    Guid Id,
    Guid OrderId,
    Money Amount,
    PaymentMethod Method,
    PaymentStatus Status,
    DateTime CreatedOn
);