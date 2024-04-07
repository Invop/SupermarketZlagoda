using Zlagoda.Application.Models;

namespace Zlagoda.Application.Services;

public interface IStoreProductService
{
    Task<bool> CreateAsync(StoreProduct storeProduct);
    Task<StoreProduct?> GetByUpcAsync(string upc);
    Task<StoreProduct?> GetByPromoUpcAsync(string upc);
    
    Task<IEnumerable<StoreProduct>> GetAllAsync();
    Task<IEnumerable<StoreProduct>> GetAllPromoStoreProductsAsync();
    Task<int> GetQuantityByUpcPromAsync(string upc);
    Task<StoreProduct?> UpdateAsync(StoreProduct product,string prevUpc);
    Task<bool> DeleteByUpcAsync(string upc);
}