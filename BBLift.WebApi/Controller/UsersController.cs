using System.Security.Claims;
using BBLift.Application.Common;
using BBLift.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize(Roles = Roles.SuperAdmin)]
[Route("api/users")]
[ApiController]
public class UsersController(IUserManagementService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var response = await service.GetAllAsync();

        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost("{id:guid}/roles/{role}")]
    public async Task<IActionResult> AddRoleAsync(Guid id, string role)
    {
        var response = await service.AddToRoleAsync(id, role);

        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("{id:guid}/roles/{role}")]
    public async Task<IActionResult> RemoveRoleAsync(Guid id, string role)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var currentUserId))
            return Unauthorized();

        var response = await service.RemoveFromRoleAsync(id, role, currentUserId);

        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }
}