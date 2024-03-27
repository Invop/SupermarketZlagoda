using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface IEmployeeRepository
{
    Task<bool> CreateAsync(Employee employee);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<bool> UpdateAsync(Employee employee);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}