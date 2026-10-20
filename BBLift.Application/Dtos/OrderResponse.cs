using BBLift.Domain.Enums;

namespace BBLift.Application.Dtos;

public record OrderResponse(
    Guid Id,
    Guid AppUserId,
    Guid MembershipPackageId,
    string PackageName,
    decimal TotalAmount,
    string Currency,
    OrderStatus Status,
    Guid? PaymentId
);