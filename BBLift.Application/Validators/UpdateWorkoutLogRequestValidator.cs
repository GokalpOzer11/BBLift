using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class UpdateWorkoutLogRequestValidator : AbstractValidator<UpdateWorkoutLogRequest>
{
    public UpdateWorkoutLogRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Güncellenecek set kaydının ID bilgisi gereklidir.");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0).WithMessage("Ağırlık 0 veya 0'dan büyük olmalıdır.");

        RuleFor(x => x.Reps)
            .GreaterThanOrEqualTo(0).WithMessage("Tekrar sayısı 0 veya daha fazla olmalıdır.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Geçerli bir set durumu seçmelisiniz.");
        
        RuleFor(x => x.RestTimeInSeconds)
            .GreaterThanOrEqualTo(0).When(x => x.RestTimeInSeconds.HasValue)
            .WithMessage("Dinlenme süresi 0 veya 0'dan büyük olmalıdır.");
    }
}