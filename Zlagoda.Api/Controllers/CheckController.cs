using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Contracts.QueryParameters;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.Requests;

namespace Zlagoda.Api.Controllers;

[ApiController]
public class CheckController : ControllerBase
{
    private readonly ICheckService _checkService;

    public CheckController(ICheckService checkService)
    {
        _checkService = checkService;
    }
    
    [HttpPost(ApiEndpoints.Checks.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCheckRequest request)
    {
        var check = request.MapToCheck();
        await _checkService.CreateAsync(check);
        return CreatedAtAction(nameof(Get), new { id = check.IdCheck }, check);
    }

    [HttpGet(ApiEndpoints.Checks.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var check = await _checkService.GetByIdAsync(id);
        if (check == null)
        {
            return NotFound();
        }
        return Ok(check.MapToCheckResponse());
    }
    
    [HttpGet(ApiEndpoints.Checks.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] CheckQueryParameters? parameters)
    {
        var checks = await _checkService.GetAllAsync(parameters);
        var checksResponse = checks.MapToChecksResponse();
        return Ok(checksResponse);
    }
    
    
    [HttpDelete(ApiEndpoints.Checks.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _checkService.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}