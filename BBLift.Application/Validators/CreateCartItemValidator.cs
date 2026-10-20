using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateCartItemValidator
    : AbstractValidator<CreateCartItemRequest>
{
    public CreateCartItemValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("Cart Id is required.");
        RuleFor(x => x.MembershipPackageId)
            .NotEmpty()
            .WithMessage("Product Variant Id is required.");
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}
