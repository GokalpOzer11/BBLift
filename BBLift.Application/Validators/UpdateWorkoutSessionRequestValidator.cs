using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class UpdateWorkoutSessionRequestValidator : AbstractValidator<UpdateWorkoutSessionRequest>
{
    public UpdateWorkoutSessionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Güncellenecek antrenman oturumunun ID bilgisi gereklidir.");

        RuleFor(x => x.WorkOutDurationInSeconds)
            .GreaterThanOrEqualTo(0).WithMessage("Antrenman süresi 0 veya 0'dan büyük olmalıdır.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Geçerli bir antrenman durumu seçmelisiniz.");
        
    }
}