using BBLift.Application.Contracts;
using BBLift.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBLift.WebApi.Controllers;

[Authorize] 
[Route("api/favorite-exercises")]
[ApiController]
public class FavoriteExercisesController(IFavoriteExerciseService service) : ControllerBase
{
    [HttpGet("get-all-by-user-id")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var response = await service.GetByUserIdAsync(id);
        
        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateFavoriteExerciseRequest request)
    {
        var response = await service.CreateAsync(request);
        
        return response.IsSuccess
            ? Created() 
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