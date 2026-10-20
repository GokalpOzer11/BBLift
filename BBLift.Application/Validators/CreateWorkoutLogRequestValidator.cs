using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateWorkoutLogRequestValidator : AbstractValidator<CreateWorkoutLogRequest>
{
    public CreateWorkoutLogRequestValidator()
    {
        RuleFor(x => x.WorkoutSessionId)
            .NotEmpty().WithMessage("Antrenman oturumu (WorkoutSessionId) boş bırakılamaz.");

        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage("Egzersiz (ExerciseId) boş bırakılamaz.");

        RuleFor(x => x.SetNumber)
            .GreaterThan(0).WithMessage("Set numarası 0'dan büyük olmalıdır.");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0).WithMessage("Ağırlık 0 veya 0'dan büyük olmalıdır.");

        RuleFor(x => x.Reps)
            .GreaterThanOrEqualTo(0).WithMessage("Tekrar sayısı 0 veya daha fazla olmalıdır.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Geçerli bir set durumu seçmelisiniz.");

        RuleFor(x => x.Rir)
            .InclusiveBetween(0, 10).When(x => x.Rir.HasValue)
            .WithMessage("RIR (Tükenmişliğe Kalan Tekrar) değeri 0 ile 10 arasında olmalıdır.");

        RuleFor(x => x.RestTimeInSeconds)
            .GreaterThanOrEqualTo(0).When(x => x.RestTimeInSeconds.HasValue)
            .WithMessage("Dinlenme süresi 0 veya 0'dan büyük olmalıdır.");
    }
}