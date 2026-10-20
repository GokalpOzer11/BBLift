using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Domain.ValueObjects;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;

namespace BBLift.Application.Features;

public class MembershipPackageService(
    IMembershipPackageRepository repository,
    IValidator<CreateMembershipPackageRequest> createValidator,
    IValidator<UpdateMembershipPackageRequest> updateValidator
    ) : IMembershipPackageService
{
    public async Task<Result<MembershipPackageResponse>> CreateAsync(CreateMembershipPackageRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<MembershipPackageResponse>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var package = await repository.AddAsync(new MembershipPackage
        {
            Name = request.Name,
            Description = request.Description,
            Price = Money.TRY(request.Price), 
            DurationInDays = request.DurationInMonths * 30 
        });

        return Result<MembershipPackageResponse>.Success(new MembershipPackageResponse(
            package.Id,
            package.Name,
            package.Description,
            package.Price.Amount, 
            package.Price.Currency, 
            package.DurationInDays / 30 
        ));
    }

    public async Task<Result> UpdateAsync(UpdateMembershipPackageRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var package = await repository.GetAsync(predicate: x => x.Id == request.Id, ct: ct);
        if (package is null)
            return Result.Failure("Abonelik paketi bulunamadı.");

        package.Name = request.Name;
        package.Description = request.Description;
        package.Price = Money.TRY(request.Price); 
        package.DurationInDays = request.DurationInMonths * 30;

        await repository.UpdateAsync(package);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<MembershipPackageResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var packages = await repository.GetListAsync(ct: ct);
        if (!packages.Any())
            return Result<IEnumerable<MembershipPackageResponse>>.Failure("Sistemde kayıtlı abonelik paketi bulunmuyor.");

        return Result<IEnumerable<MembershipPackageResponse>>.Success(
            [.. packages.Select(package => new MembershipPackageResponse(
                package.Id,
                package.Name,
                package.Description,
                package.Price.Amount,
                package.Price.Currency,
                package.DurationInDays / 30
            ))]
        );
    }

    public async Task<Result<MembershipPackageResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var package = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (package is null)
            return Result<MembershipPackageResponse>.Failure("Abonelik paketi bulunamadı.");

        return Result<MembershipPackageResponse>.Success(new MembershipPackageResponse(
            package.Id,
            package.Name,
            package.Description,
            package.Price.Amount,
            package.Price.Currency,
            package.DurationInDays / 30
        ));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var package = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (package is null)
            return Result.Failure("Abonelik paketi bulunamadı.");

        await repository.DeleteAsync(package);
        return Result.Success();
    }
}