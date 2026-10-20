using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize]
[Route("api/cart-items")]
[ApiController]
public class CartItemsController(ICartItemService service) : ControllerBase
{
    [HttpGet("by-cart/{cartId:guid}")]
    public async Task<IActionResult> GetByCartIdAsync(Guid cartId)
    {
        var response = await service.GetByCartIdAsync(cartId);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateCartItemRequest request)
    {
        var response = await service.CreateAsync(request);
        
        return response.IsSuccess
            ? Created() 
            : BadRequest(response.ErrorMessage);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateCartItemRequest request)
    {
        if (id != request.Id)
            return BadRequest("URL ID does not match Body ID"); 

        var response = await service.UpdateAsync(request);
        
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