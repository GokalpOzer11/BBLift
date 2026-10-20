using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateOrderValidator
    : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı Id'si Zorunlu.");
        
    }
}
