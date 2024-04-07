using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface ICheckRepository
{
    Task<bool> CreateAsync(Check check);
    Task<Check?> GetByIdAsync(Guid id);
    Task<IEnumerable<Check>> GetAllAsync();
    Task<bool> DeleteByIdAsync(Guid id);
    
}