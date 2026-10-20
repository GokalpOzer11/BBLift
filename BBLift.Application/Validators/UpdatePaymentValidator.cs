using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class UpdatePaymentValidator
    : AbstractValidator<UpdatePaymentRequest>
{
    public UpdatePaymentValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("Id Zorunludur.");
        RuleFor(x => x.Status)
            .NotEmpty()
            .IsInEnum();
    }
}
