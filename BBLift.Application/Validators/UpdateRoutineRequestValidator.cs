using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class UpdateRoutineRequestValidator : AbstractValidator<UpdateRoutineRequest>
{
    public UpdateRoutineRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Antrenman programı adı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Antrenman programı adı en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Antrenman programı adı en fazla 100 karakter olabilir.");
    }
}