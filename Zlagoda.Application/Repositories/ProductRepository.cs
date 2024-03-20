using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public class ProductRepository : IProductRepository
{
    public async Task<bool> CreateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }


    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
}