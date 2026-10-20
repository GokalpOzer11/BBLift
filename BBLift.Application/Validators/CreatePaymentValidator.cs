using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreatePaymentValidator
    : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Sipariş Id'si zorunludur.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Ödeme tutarı 0'dan büyük olmalıdır.");

        RuleFor(x => x.Method)
            .IsInEnum()
            .WithMessage("Geçerli bir ödeme yöntemi seçmelisiniz.");
    }
}