using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize]
[Route("api/orders")]
[ApiController]
public class OrdersController(IOrderService service) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await service.GetByIdAsync(id);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpGet("get-all-by-user-id")]
    public async Task<IActionResult> GetByUserIdAsync(string userId)
    {
        var response = await service.GetByUserIdAsync(userId);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateOrderRequest request)
    {
        var response = await service.CreateAsync(request);
        
        return response.IsSuccess
            ? Created($"/api/orders/{response.Data!.Id}", response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateOrderRequest request)
    {
        if (id != request.Id)
            return BadRequest("URL ID does not match Body ID");

        var response = await service.UpdateStatusAsync(request);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var response = await service.DeleteAsync(id);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }
}
