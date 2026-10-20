using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class CreateMembershipPackageRequestValidator : AbstractValidator<CreateMembershipPackageRequest>
{
    public CreateMembershipPackageRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Paket adı boş bırakılamaz.")
            .MinimumLength(2).WithMessage("Paket adı en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Paket adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Paket açıklaması en fazla 500 karakter olabilir.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Paket fiyatı 0 veya 0'dan büyük olmalıdır.");

        RuleFor(x => x.DurationInMonths)
            .GreaterThan(0).WithMessage("Paket süresi en az 1 ay olmalıdır.");
    }
}