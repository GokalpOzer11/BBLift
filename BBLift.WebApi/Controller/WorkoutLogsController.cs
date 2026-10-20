using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize]
[Route("api/workout-logs")]
[ApiController]
public class WorkoutLogsController(IWorkoutLogService service) : ControllerBase
{
    [HttpGet("by-session/{sessionId:guid}")]
    public async Task<IActionResult> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var response = await service.GetBySessionIdAsync(sessionId, cancellationToken);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateWorkoutLogRequest request, CancellationToken cancellationToken)
    {
        var response = await service.CreateAsync(request, cancellationToken);
        
        return response.IsSuccess
            ? Created()
            : BadRequest(response.ErrorMessage);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateWorkoutLogRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest("URL ID ile İstek (Body) ID'si eşleşmiyor.");

        var response = await service.UpdateAsync(request, cancellationToken);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await service.DeleteAsync(id, cancellationToken);
        
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response.ErrorMessage);
    }
}