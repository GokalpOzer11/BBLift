using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateMuscleGroupRequestValidator : AbstractValidator<CreateMuscleGroupRequest>
{
    public CreateMuscleGroupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kas grubu adı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Kas grubu adı en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Kas grubu adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Görsel URL'si boş bırakılamaz.")
            .MaximumLength(500).WithMessage("Görsel URL'si en fazla 500 karakter olabilir.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("Geçerli bir URL girmelisiniz.");
    }
}