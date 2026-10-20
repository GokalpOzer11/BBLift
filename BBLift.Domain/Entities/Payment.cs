using BBLift.Domain.Common;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;

namespace BBLift.Domain.Entities;

public class Payment : EntityBase
{
    public Guid OrderId { get; set; }
    public Money Amount { get; set; } = Money.Zero();
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }

    public Order? Order { get; set; }
}