using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface IEmployeeRepository
{
    Task<bool> CreateAsync(Employee employee);
    Task<Employee?> GetByIdAsync(string id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<bool> UpdateAsync(Employee employee);
    Task<bool> DeleteByIdAsync(string id);
    Task<bool> ExistsByIdAsync(string id);
}