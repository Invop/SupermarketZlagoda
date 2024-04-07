using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;

namespace Zlagoda.Application.Services;

public class StoreProductService : IStoreProductService
{
    private readonly IStoreProductRepository _storeProductRepository;

    public StoreProductService(IStoreProductRepository storeProductRepository)
    {
        _storeProductRepository = storeProductRepository;
    }

    public async Task<bool> CreateAsync(StoreProduct storeProduct)
    {
       return await _storeProductRepository.CreateAsync(storeProduct);
    }

    public async Task<StoreProduct?> GetByUpcAsync(string upc)
    {
        return await _storeProductRepository.GetByUpcAsync(upc);
    }

    public async Task<StoreProduct?> GetByPromoUpcAsync(string upc)
    {
        return await _storeProductRepository.GetByPromoUpcAsync(upc);
    }

    public async Task<IEnumerable<StoreProduct>> GetAllAsync()
    {
       return await _storeProductRepository.GetAllAsync();
    }

    public async Task<IEnumerable<StoreProduct>> GetAllPromoStoreProductsAsync()
    {
        return await _storeProductRepository.GetAllPromoStoreProductsAsync();
    }

    public async Task<int> GetQuantityByUpcPromAsync(string upc)
    {
        return await _storeProductRepository.GetQuantityByUpcPromAsync(upc);
    }

    public async Task<StoreProduct?> UpdateAsync(StoreProduct product,string prevUpc)
    {
        var storeProductExists = await _storeProductRepository.ExistsByUpcAsync(prevUpc);
        if (!storeProductExists) 
            return null;
        
        await _storeProductRepository.UpdateAsync(product,prevUpc);
        if(!string.IsNullOrEmpty(product.UpcProm))
        {
            await _storeProductRepository.UpdatePromProductIdAsync(product.ProductId,product.UpcProm);
        }
        await _storeProductRepository.UpdatePromUpcAsync(prevUpc, product.Upc);
        return  product;
    }

    public async Task<bool> DeleteByUpcAsync(string upc)
    {
        
        await _storeProductRepository.UpdatePromUpcAsync(upc, null);
        return await _storeProductRepository.DeleteByUpcAsync(upc);
        
    }
}