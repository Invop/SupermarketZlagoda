using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface IStoreProductRepository
{
    Task<bool> CreateAsync(StoreProduct storeProduct);
    Task<StoreProduct?> GetByUPCAsync(string upc);
    Task<IEnumerable<StoreProduct>> GetAllAsync();
    Task<bool> UpdatePromProductsAsync(string prevUpc, string newUpc);
    Task<bool> UpdateAsync(StoreProduct product, string prevUpc);
    Task<bool> DeleteByUPCAsync(string upc);
    Task<bool> ExistsByUPCAsync(string upc);
}