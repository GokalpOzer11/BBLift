using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize(Roles = "SuperAdmin, Admin")] 
[Route("api/equipments")]
[ApiController]
public class EquipmentsController(IEquipmentService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await service.GetByIdAsync(id);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [AllowAnonymous] 
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var response = await service.GetAllAsync();
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateEquipmentRequest request)
    {
        var response = await service.CreateAsync(request);
        
        return response.IsSuccess
            ? Created($"/api/equipments/{response.Data!.Id}", response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateEquipmentRequest request)
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