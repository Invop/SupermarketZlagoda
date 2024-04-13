using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public interface IStoreProductRepository
{
    Task<bool> CreateAsync(StoreProduct storeProduct);
    Task<StoreProduct?> GetByUpcAsync(string upc);
    Task<StoreProduct?> GetByPromoUpcAsync(string upc);
    Task<IEnumerable<StoreProduct>> GetAllAsync(StoreProductQueryParameters? parameters);
    Task<int> GetQuantityByUpcPromAsync(string upc);
    Task<bool> UpdatePromUpcAsync(string prevUpc, string? newUpc);
    Task<bool> UpdatePromProductIdAsync(Guid productId, string upcProm);
    Task<bool> UpdateAsync(StoreProduct product, string prevUpc);
    Task<bool> DeleteByUpcAsync(string upc);
    Task<bool> ExistsByUpcAsync(string upc);

    Task<IEnumerable<StoreProduct>> GetAllPromoProductsAsync();

    Task<IEnumerable<StoreProduct>> GetAllNotPromoProductsAsync();

}