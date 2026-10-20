using BBLift.Application.Common;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BBLift.Application.Features;

public class UserManagementService(UserManager<AppUser> userManager) : IUserManagementService
{
    private static readonly string[] AssignableRoles = [Roles.SuperAdmin, Roles.Admin, Roles.User];

    private static string? Canonical(string role) =>
        AssignableRoles.FirstOrDefault(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));

    public async Task<Result<IEnumerable<UserResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await userManager.Users.OrderBy(u => u.UserName).ToListAsync(ct);

        var response = new List<UserResponse>();
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            response.Add(new UserResponse(
                user.Id,
                user.UserName ?? string.Empty,
                user.Email ?? string.Empty,
                user.FirstName,
                user.LastName,
                [.. roles.OrderBy(r => r)]));
        }

        return Result<IEnumerable<UserResponse>>.Success(response);
    }

    public async Task<Result> AddToRoleAsync(Guid userId, string role, CancellationToken ct = default)
    {
        var roleName = Canonical(role);
        if (roleName is null)
            return Result.Failure("Geçersiz rol.");

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("Kullanıcı bulunamadı.");

        if (await userManager.IsInRoleAsync(user, roleName))
            return Result.Failure("Kullanıcı zaten bu role sahip.");

        var result = await userManager.AddToRoleAsync(user, roleName);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<Result> RemoveFromRoleAsync(Guid userId, string role, Guid currentUserId, CancellationToken ct = default)
    {
        var roleName = Canonical(role);
        if (roleName is null)
            return Result.Failure("Geçersiz rol.");

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure("Kullanıcı bulunamadı.");

        if (!await userManager.IsInRoleAsync(user, roleName))
            return Result.Failure("Kullanıcı bu role sahip değil.");

        if (roleName == Roles.SuperAdmin)
        {
            if (userId == currentUserId)
                return Result.Failure("Kendi SuperAdmin rolünü kaldıramazsın.");

            var superAdmins = await userManager.GetUsersInRoleAsync(Roles.SuperAdmin);
            if (superAdmins.Count <= 1)
                return Result.Failure("Sistemde en az bir SuperAdmin kalmalıdır.");
        }

        var result = await userManager.RemoveFromRoleAsync(user, roleName);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}