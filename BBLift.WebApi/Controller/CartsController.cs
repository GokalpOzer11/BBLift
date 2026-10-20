using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize]
[Route("api/carts")]
[ApiController]
public class CartsController(ICartService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await service.GetByIdAsync(id);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpGet("get-by-user-id")]
    public async Task<IActionResult> GetByUserIdAsync(string userId)
    {
        var response = await service.GetByUserIdAsync(userId);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateCartRequest request)
    {
        var response = await service.CreateAsync(request);
        
        return response.IsSuccess
            ? Created($"/api/carts/{response.Data!.Id}", response.Data)   
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost("add-item")]
    public async Task<IActionResult> AddItemAsync(CreateCartItemRequest request)
    {
        var response = await service.AddItemAsync(request);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }

    [HttpPut("update-item")]
    public async Task<IActionResult> UpdateItemAsync(UpdateCartItemRequest request)
    {
        var response = await service.UpdateItemAsync(request);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("remove-item/{id:guid}")]
    public async Task<IActionResult> RemoveItemAsync(Guid id)
    {
        var response = await service.RemoveItemAsync(id);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("clear-cart")]
    public async Task<IActionResult> ClearAsync([FromQuery] string userId)
    {
        var response = await service.ClearAsync(userId);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }
}