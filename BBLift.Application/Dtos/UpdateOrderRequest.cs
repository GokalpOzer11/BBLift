using BBLift.Domain.Enums;

namespace BBLift.Application.Dtos;

public record UpdateOrderRequest(
    Guid Id,
    OrderStatus Status
);