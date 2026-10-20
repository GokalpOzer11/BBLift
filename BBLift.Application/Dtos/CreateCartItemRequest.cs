namespace BBLift.Application.Dtos;

public record CreateCartItemRequest
(
    Guid CartId,
    Guid MembershipPackageId,
    int Quantity
);