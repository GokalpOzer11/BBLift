using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateFavoriteExerciseRequestValidator : AbstractValidator<CreateFavoriteExerciseRequest>
{
    public CreateFavoriteExerciseRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID bilgisi boş olamaz.");

        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage("Favorilere eklenecek geçerli bir egzersiz seçmelisiniz.");
    }
}