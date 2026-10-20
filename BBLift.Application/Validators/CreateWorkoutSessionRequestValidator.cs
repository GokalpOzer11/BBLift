using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateWorkoutSessionRequestValidator : AbstractValidator<CreateWorkoutSessionRequest>
{
    public CreateWorkoutSessionRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID bilgisi boş olamaz.");
    }
}