using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<bool> CreateAsync(Employee employee)
    {
        return await _employeeRepository.CreateAsync(employee);
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _employeeRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(EmployeeQueryParameters? parameters)
    {
        return await _employeeRepository.GetAllAsync(parameters);
    }

    public async Task<Employee?> UpdateAsync(Employee employee)
    {
        var productExists = await _employeeRepository.ExistsByIdAsync(employee.Id);
        if (!productExists) return null;
        await _employeeRepository.UpdateAsync(employee);
        return employee;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await _employeeRepository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<Employee>> GetCashiersServedAllCustomers()
    {
        return await _employeeRepository.GetCashiersServedAllCustomers();
    }
}