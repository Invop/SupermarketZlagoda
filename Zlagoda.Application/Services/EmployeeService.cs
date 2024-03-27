using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;

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

    public async Task<Employee?> GetByIdAsync(string id)
    {
        return await _employeeRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _employeeRepository.GetAllAsync();
    }

    public async Task<Employee?> UpdateAsync(Employee employee)
    {
        var productExists = await _employeeRepository.ExistsByIdAsync(employee.Id);
        if (!productExists) return null;
        await _employeeRepository.UpdateAsync(employee);
        return employee;
    }

    public async Task<bool> DeleteByIdAsync(string id)
    {
        return await _employeeRepository.DeleteByIdAsync(id);
    }
}