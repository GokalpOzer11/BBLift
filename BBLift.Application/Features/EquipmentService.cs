using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;

namespace BBLift.Application.Features;

public class EquipmentService(
    IEquipmentRepository repository,
    IValidator<CreateEquipmentRequest> createValidator,
    IValidator<UpdateEquipmentRequest> updateValidator
    ) : IEquipmentService
{
    public async Task<Result<EquipmentResponse>> CreateAsync(CreateEquipmentRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<EquipmentResponse>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var equipment = await repository.AddAsync(new Equipment
        {
            Name = request.Name,
        });

        return Result<EquipmentResponse>.Success(new EquipmentResponse(
            equipment.Id,
            equipment.Name
        ));
    }

    public async Task<Result> UpdateAsync(UpdateEquipmentRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var equipment = await repository.GetAsync(predicate: x => x.Id == request.Id, ct: ct);
        if (equipment is null)
            return Result.Failure("Ekipman bulunamadı.");

        equipment.Name = request.Name;

        await repository.UpdateAsync(equipment);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<EquipmentResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var equipments = await repository.GetListAsync(ct: ct);
        if (!equipments.Any())
            return Result<IEnumerable<EquipmentResponse>>.Failure("Sistemde kayıtlı ekipman bulunmuyor.");

        return Result<IEnumerable<EquipmentResponse>>.Success(
            [.. equipments.Select(x => new EquipmentResponse(
                x.Id,
                x.Name
            ))]
        );
    }

    public async Task<Result<EquipmentResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var equipment = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (equipment is null)
            return Result<EquipmentResponse>.Failure("Ekipman bulunamadı.");

        return Result<EquipmentResponse>.Success(new EquipmentResponse(
            equipment.Id,
            equipment.Name
        ));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var equipment = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (equipment is null)
            return Result.Failure("Ekipman bulunamadı.");

        await repository.DeleteAsync(equipment);
        return Result.Success();
    }
}