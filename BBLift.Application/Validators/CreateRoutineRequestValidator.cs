using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateRoutineRequestValidator : AbstractValidator<CreateRoutineRequest>
{
    public CreateRoutineRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID bilgisi boş olamaz.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Antrenman programı adı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Antrenman programı adı en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Antrenman programı adı en fazla 100 karakter olabilir.");
    }
}