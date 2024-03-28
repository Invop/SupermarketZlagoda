using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.Requests;

namespace Zlagoda.Api.Controllers;

public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    [HttpPost(ApiEndpoints.Employees.Create)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        var employee = request.MapToEmployee();
        await _employeeService.CreateAsync(employee);
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [HttpGet(ApiEndpoints.Employees.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee.MapToResponse());
    }
    
    [HttpGet(ApiEndpoints.Employees.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        var employeesResponse = employees.MapToResponse();
        return Ok(employeesResponse);
    }


    [HttpPut(ApiEndpoints.Employees.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateEmployeeRequest request)
    {
        var employee = request.MapToEmployee(id);
        var updatedProduct = await _employeeService.UpdateAsync(employee);
        if (updatedProduct is null) return NotFound();
        return Ok(employee.MapToResponse());
    }
    
    
    [HttpDelete(ApiEndpoints.Employees.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _employeeService.DeleteByIdAsync(id);
        if (!deleted) return NotFound();
        return Ok();
    }
}