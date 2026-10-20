using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Contracts;
using FluentValidation;

namespace BBLift.Application.Features;

public class MuscleGroupService(
    IMuscleGroupRepository repository,
    IValidator<CreateMuscleGroupRequest> createValidator,
    IValidator<UpdateMuscleGroupRequest> updateValidator
) : IMuscleGroupService
{
    public async Task<Result<MuscleGroupResponse>> CreateAsync(CreateMuscleGroupRequest request, CancellationToken ct = default)
    {
        var validationResult = await createValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<MuscleGroupResponse>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var muscleGroup = await repository.AddAsync(new MuscleGroup
        {
            Name = request.Name,
            ImageUrl = request.ImageUrl
        });

        return Result<MuscleGroupResponse>.Success(new MuscleGroupResponse(
            muscleGroup.Id,
            muscleGroup.Name,
            muscleGroup.ImageUrl
        ));
    }

    public async Task<Result> UpdateAsync(UpdateMuscleGroupRequest request, CancellationToken ct = default)
    {
        var validationResult = await updateValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var muscleGroup = await repository.GetAsync(predicate: x => x.Id == request.Id, ct: ct);
        if (muscleGroup is null)
            return Result.Failure("Kas grubu bulunamadı.");

        muscleGroup.Name = request.Name;
        muscleGroup.ImageUrl = request.ImageUrl;

        await repository.UpdateAsync(muscleGroup);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<MuscleGroupResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var muscleGroups = await repository.GetListAsync(ct: ct);
        if (!muscleGroups.Any())
            return Result<IEnumerable<MuscleGroupResponse>>.Failure("Kas grubu kaydı bulunamadı.");

        return Result<IEnumerable<MuscleGroupResponse>>.Success(
            [.. muscleGroups.Select(x => new MuscleGroupResponse(
                x.Id,
                x.Name,
                x.ImageUrl
            ))]
        );
    }

    public async Task<Result<MuscleGroupResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var muscleGroup = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (muscleGroup is null)
            return Result<MuscleGroupResponse>.Failure("Kas grubu bulunamadı.");

        return Result<MuscleGroupResponse>.Success(new MuscleGroupResponse(
            muscleGroup.Id,
            muscleGroup.Name,
            muscleGroup.ImageUrl
        ));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var muscleGroup = await repository.GetAsync(predicate: x => x.Id == id, ct: ct);
        if (muscleGroup is null)
            return Result.Failure("Kas grubu bulunamadı.");

        await repository.DeleteAsync(muscleGroup);
        return Result.Success();
    }
}