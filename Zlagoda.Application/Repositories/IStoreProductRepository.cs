using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface IStoreProductRepository
{
    Task<bool> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}