using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize]
[Route("api/routines")]
[ApiController]
public class RoutinesController(IRoutineService service) : ControllerBase
{
    [HttpGet("{id:guid}", Name = "GetRoutineById")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await service.GetByIdAsync(id);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var response = await service.GetAllAsync();
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpGet("get-all-by-user-id")]
    public async Task<IActionResult> GetByUserIdAsync(string id)
    {
        var response = await service.GetByUserIdAsync(id);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateRoutineRequest request)
    {
        var response = await service.CreateAsync(request);
        
        return response.IsSuccess
            ? CreatedAtRoute("GetRoutineById", new { id = response.Data!.Id }, response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateRoutineRequest request)
    {
        if (id != request.RoutineId)
            return BadRequest("URL ID does not match Body ID");

        var response = await service.UpdateAsync(id, request);
        
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