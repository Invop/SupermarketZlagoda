using Zlagoda.Application.Models;

namespace Zlagoda.Application.Services;

public interface IStoreProductService
{
    Task<bool> CreateAsync(StoreProduct storeProduct);
    Task<StoreProduct?> GetByUPCAsync(string upc);
    Task<IEnumerable<StoreProduct>> GetAllAsync();
    Task<StoreProduct?> UpdateAsync(StoreProduct product,string prevUpc);
    Task<bool> DeleteByUPCAsync(string upc);
}