using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public interface IProductRepository
{
    Task<bool> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync(ProductQueryParameters? parameters);
    Task<IEnumerable<Product>> GetAllSortedDescendingAsync();
    Task<IEnumerable<Product>> GetAllUnusedAsync();
    Task<IEnumerable<Product>> GetAllUnusedAndCurrentAsync(Guid id);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}