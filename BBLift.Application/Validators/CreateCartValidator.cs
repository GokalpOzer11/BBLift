using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateCartValidator
    : AbstractValidator<CreateCartRequest>
{
    public CreateCartValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcının Id'si Zorunlu");
    }
}