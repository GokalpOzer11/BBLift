using System.Security.Claims;
using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Route("auth")]
[ApiController]
public class AuthController(IAuthService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync(SignInRequest request)
    {
        var response = await service.SignInAsync(request);
        
        return response.IsSuccess 
            ? Ok(response) 
            : BadRequest(response); 
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync(SignUpRequest request)
    {
        var response = await service.SignUpAsync(request);
        
        return response.IsSuccess
            ? Created(string.Empty, response) 
            : BadRequest(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(TokenModel request)
    {
        var response = await service.RefreshAsync(request);
        
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    [HttpGet("revoke/{userId}")]
    [Authorize]
    public async Task<IActionResult> RevokeAsync(string userId)
    {
        var response = await service.RevokeAsync(userId);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response); 
    }
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfileAsync()
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        var response = await service.GetProfileAsync(userId!);
        
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileRequest request)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        var response = await service.UpdateProfileAsync(userId!, request);
        
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        var response = await service.ChangePasswordAsync(userId!, request);
        
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}