using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface ICategoryRepository
{
    Task<bool> CreateAsync(Category category);
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<bool> UpdateAsync(Category category);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}