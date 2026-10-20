using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class UpdateOrderValidator
    : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id Zorunludur.");
        RuleFor(x => x.Status)
            .NotEmpty()
            .IsInEnum();
    }
}