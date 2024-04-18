using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public interface ICategoryService
{
    Task<bool> CreateAsync(Category product);
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync(CategoryQueryParameters? parameters);
    Task<Category?> UpdateAsync(Category category);
    Task<bool> DeleteByIdAsync(Guid id);
}