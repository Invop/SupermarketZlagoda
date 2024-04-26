using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.QueryParameters;
using Zlagoda.Contracts.Requests;

namespace Zlagoda.Api.Controllers;

public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpPost(ApiEndpoints.Employees.Create)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        var employee = request.MapToEmployee();
        await employeeService.CreateAsync(employee);
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [HttpGet(ApiEndpoints.Employees.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var employee = await employeeService.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee.MapToEmployeeResponse());
    }
    
    [HttpGet(ApiEndpoints.Employees.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] EmployeeQueryParameters? parameters)
    {
        var employees = await employeeService.GetAllAsync(parameters);
        var employeesResponse = employees.MapToEmployeeResponse();
        return Ok(employeesResponse);
    }


    [HttpPut(ApiEndpoints.Employees.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateEmployeeRequest request)
    {
        var employee = request.MapToEmployee(id);
        var updatedProduct = await employeeService.UpdateAsync(employee);
        if (updatedProduct is null) return NotFound();
        return Ok(employee.MapToEmployeeResponse());
    }
    
    
    [HttpDelete(ApiEndpoints.Employees.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await employeeService.DeleteByIdAsync(id);
        if (!deleted) return NotFound();
        return Ok();
    }
    
    [HttpGet(ApiEndpoints.Employees.GetCashiersServedAllCustomers)]
    public async Task<IActionResult> GetAll()
    {
        var employees = await employeeService.GetCashiersServedAllCustomers();
        var employeesResponse = employees.MapToEmployeeResponse();
        return Ok(employeesResponse);
    }
}