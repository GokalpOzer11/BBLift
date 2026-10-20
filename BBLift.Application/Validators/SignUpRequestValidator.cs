using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class SignUpRequestValidator 
    : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad alanı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Ad en az 2 karakter olmalıdır.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta alanı boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş bırakılamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Doğum tarihi boş bırakılamaz.");

    }
}