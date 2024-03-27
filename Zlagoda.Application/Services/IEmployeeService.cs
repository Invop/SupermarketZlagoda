using Zlagoda.Application.Models;

namespace Zlagoda.Application.Services;

public interface IEmployeeService
{
    Task<bool> CreateAsync(Employee employee);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> UpdateAsync(Employee employee);
    Task<bool> DeleteByIdAsync(Guid id);
}