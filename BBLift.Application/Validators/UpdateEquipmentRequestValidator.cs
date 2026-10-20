using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class UpdateEquipmentRequestValidator : AbstractValidator<UpdateEquipmentRequest>
{
    public UpdateEquipmentRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Güncellenecek ekipmanın ID bilgisi gereklidir.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ekipman adı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Ekipman adı en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Ekipman adı en fazla 100 karakter olabilir.");
    }
}