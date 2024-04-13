using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public interface IStoreProductService
{
    Task<bool> CreateAsync(StoreProduct storeProduct);
    Task<StoreProduct?> GetByUpcAsync(string upc);
    Task<StoreProduct?> GetByPromoUpcAsync(string upc);
    
    Task<IEnumerable<StoreProduct>> GetAllAsync(StoreProductQueryParameters? parameters);
    Task<int> GetQuantityByUpcPromAsync(string upc);
    Task<StoreProduct?> UpdateAsync(StoreProduct product,string prevUpc);
    Task<bool> DeleteByUpcAsync(string upc);

}