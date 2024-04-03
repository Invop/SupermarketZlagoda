using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface IStoreProductRepository
{
    Task<bool> CreateAsync(StoreProduct storeProduct);
    Task<StoreProduct?> GetByUPCAsync(string upc);
    Task<IEnumerable<StoreProduct>> GetAllAsync();
    Task<int> GetQuantityByUpcPromAsync(string upc);
    Task<bool> UpdatePromUpcAsync(string prevUpc, string? newUpc);
    Task<bool> UpdatePromProductIdAsync(Guid productId, string upcProm);
    Task<bool> UpdateAsync(StoreProduct product, string prevUpc);
    Task<bool> DeleteByUPCAsync(string upc);
    Task<bool> ExistsByUPCAsync(string upc);

    Task<IEnumerable<StoreProduct>> GetAllPromoStoreProductsAsync();

}