using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateEquipmentRequestValidator : AbstractValidator<CreateEquipmentRequest>
{
    public CreateEquipmentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ekipman adı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Ekipman adı en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Ekipman adı en fazla 100 karakter olabilir.");
    }
}