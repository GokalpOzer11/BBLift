using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class UpdateRoutineExerciseRequestValidator : AbstractValidator<UpdateRoutineExerciseRequest>
{
    public UpdateRoutineExerciseRequestValidator()
    {
        RuleFor(x => x.RoutineId)
            .NotEmpty().WithMessage("Antrenman programı (RoutineId) boş bırakılamaz.");

        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage("Egzersiz (ExerciseId) boş bırakılamaz.");

        RuleFor(x => x.Sets)
            .GreaterThan(0).WithMessage("Set sayısı 0'dan büyük olmalıdır.");

        RuleFor(x => x.Reps)
            .GreaterThan(0).WithMessage("Tekrar sayısı 0'dan büyük olmalıdır.");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0).WithMessage("Ağırlık 0 veya 0'dan büyük olmalıdır.");

        RuleFor(x => x.RestTimeInSeconds)
            .GreaterThanOrEqualTo(0).When(x => x.RestTimeInSeconds.HasValue)
            .WithMessage("Dinlenme süresi 0 veya 0'dan büyük olmalıdır.");
    }
}