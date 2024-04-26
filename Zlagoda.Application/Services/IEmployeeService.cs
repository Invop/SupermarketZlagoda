using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public interface IEmployeeService
{
    Task<bool> CreateAsync(Employee employee);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllAsync(EmployeeQueryParameters? parameters);
    Task<Employee?> UpdateAsync(Employee employee);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetCashiersServedAllCustomers();
}