using Zlagoda.Application.Models;

namespace Zlagoda.Application.Services;

public interface IProductService
{
    Task<bool> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> UpdateAsync(Product product);
    Task<bool> DeleteByIdAsync(Guid id);
}