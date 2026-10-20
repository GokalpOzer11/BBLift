using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize]
[Route("api/routine-exercises")]
[ApiController]
public class RoutineExercisesController(IRoutineExerciseService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateRoutineExerciseRequest request)
    {
        var response = await service.CreateAsync(request);

        return response.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, new { Id = response.Data })
            : BadRequest(response.ErrorMessage);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateRoutineExerciseRequest request)
    {
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