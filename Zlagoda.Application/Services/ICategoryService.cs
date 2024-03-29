using Zlagoda.Application.Models;

namespace Zlagoda.Application.Services;

public interface ICategoryService
{
    Task<bool> CreateAsync(Category product);
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> UpdateAsync(Category product);
    Task<bool> DeleteByIdAsync(Guid id);
}