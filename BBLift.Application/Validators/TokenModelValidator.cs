using BBLift.Application.Dtos;
using FluentValidation;

namespace BBLift.Application.Features.Validators;

public class TokenModelValidator
    : AbstractValidator<TokenModel>
{
    public TokenModelValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty();
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}