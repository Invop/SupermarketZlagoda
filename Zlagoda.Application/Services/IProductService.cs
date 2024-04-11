using Zlagoda.Application.Models;

namespace Zlagoda.Application.Services;

public interface IProductService
{
    Task<bool> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllSortedAscendingAsync();
    Task<IEnumerable<Product>> GetAllSortedDescendingAsync();
    Task<IEnumerable<Product>> GetAllUnusedAsync();
    Task<IEnumerable<Product>> GetAllUnusedAndCurrentAsync(Guid id);
    Task<Product?> UpdateAsync(Product product);
    Task<bool> DeleteByIdAsync(Guid id);
}