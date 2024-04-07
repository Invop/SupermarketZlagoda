using Zlagoda.Application.Models;

namespace Zlagoda.Application.Services;

public interface ICheckService
{
    Task<bool> CreateAsync(Check check);
    Task<Check?> GetByIdAsync(Guid id);
    Task<IEnumerable<Check>> GetAllAsync();
    Task<bool> DeleteByIdAsync(Guid id);
}