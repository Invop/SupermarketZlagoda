﻿using Zlagoda.Application.Models;
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

    public async Task<StoreProduct?> GetByUPCAsync(string upc)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StoreProduct>> GetAllAsync()
    {
       return await _storeProductRepository.GetAllAsync();
    }

    public async Task<StoreProduct?> UpdateAsync(StoreProduct product,string prevUpc)
    {
        var storeProductExists = await _storeProductRepository.ExistsByUPCAsync(prevUpc);
        if (!storeProductExists) 
            return null;
        await _storeProductRepository.UpdateAsync(product);
        var isCascadeUpdateDone = await _storeProductRepository.UpdatePromProductsAsync(prevUpc, product.Upc);
        return !isCascadeUpdateDone ? null : product;
    }

    public async Task<bool> DeleteByUPCAsync(string upc)
    {
        throw new NotImplementedException();
    }
}