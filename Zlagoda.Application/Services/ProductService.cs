using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;

namespace Zlagoda.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
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

    public async Task<IEnumerable<Product>> GetAllSortedAscendingAsync()
    {
        return await _productRepository.GetAllSortedAscendingAsync();
    }
    
    public async Task<IEnumerable<Product>> GetAllSortedDescendingAsync()
    {
        return await _productRepository.GetAllSortedDescendingAsync();
    }
    
    public async Task<IEnumerable<Product>> GetAllUnusedAsync()
    {
        return await _productRepository.GetAllUnusedAsync();
    }

    public async Task<IEnumerable<Product>> GetAllUnusedAndCurrentAsync(Guid id)
    {
        return await _productRepository.GetAllUnusedAndCurrentAsync(id);
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