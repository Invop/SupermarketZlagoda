using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;

namespace Zlagoda.Application.Services;

public class StoreProductService : IStoreProductService
{
    private readonly IProductRepository _productRepository;

    public StoreProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<bool> CreateAsync(Product product)
    {
        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var productExists = await _productRepository.ExistsByIdAsync(product.Id);
        if (!productExists)
        {
            return null;
        }
        await _productRepository.UpdateAsync(product);
        return product;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await _productRepository.DeleteByIdAsync(id);
    }
}