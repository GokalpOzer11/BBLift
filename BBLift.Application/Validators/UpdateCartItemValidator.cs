using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class UpdateCartItemValidator
    : AbstractValidator<UpdateCartItemRequest>
{
    public UpdateCartItemValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id Zorunludur.");
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Miktar Zorunludur.")
            .GreaterThan(0)
            .WithMessage("Güncellenecek Miktar 0'dan Büyük Olmalıdır");
    }
}