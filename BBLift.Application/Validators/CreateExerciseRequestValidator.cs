using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Egzersiz adı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Egzersiz adı en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Egzersiz adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");

        RuleFor(x => x.MuscleGroupId)
            .NotEmpty().WithMessage("Bir kas grubu seçmelisiniz.");
        
         RuleFor(x => x.EquipmentId).NotEmpty().WithMessage("Bir ekipman seçmelisiniz.");
    }
}