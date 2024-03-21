using Zlagoda.Application.Models;
using Zlagoda.Contracts.Requests;
using Zlagoda.Contracts.Responses;

namespace Zlagoda.Api.Mapping;

public static class ContractMapping
{
    public static Product MapToProduct(this CreateProductRequest request)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CategoryId = request.CategoryId,
            Characteristics = request.Characteristics
        };
    }

    public static ProductResponse MapToResponse(this Product movie)
    {
        return new ProductResponse()
        {
            Id = movie.Id,
            Name = movie.Name,
            CategoryId = movie.CategoryId,
            Characteristics = movie.Characteristics
        };
    }
    
    public static ProductsResponse MapToResponse(this IEnumerable<Product> movies)
    {
        return new ProductsResponse
        {
            Items = movies.Select(MapToResponse)
        };
    }

    public static Product MapToProduct(this UpdateProductRequest request,Guid id)
    {
        return new Product
        {   
            Id = id,
            Name = request.Name,
            CategoryId = request.CategoryId,
            Characteristics = request.Characteristics
        };
    }
    
    
    public static StoreProduct MapToProduct(this CreateStoreProductRequest request)
    {
        throw new NotImplementedException();
        // return new StoreProduct
        // {
        // };
    }

    public static StoreProductResponse MapToResponse(this StoreProduct movie)
    {
        throw new NotImplementedException();
        // return new StoreProductResponse()
        // {
        // };
    }
    
    public static StoreProductsResponse MapToResponse(this IEnumerable<StoreProduct> movies)
    {   
        throw new NotImplementedException();
        // return new StoreProductsResponse
        // {
        //     Items = movies.Select(MapToResponse)
        // };
    }

    public static StoreProduct MapToProduct(this UpdateStoreProductRequest request,Guid id)
    {   
        throw new NotImplementedException();
        // return new StoreProduct
        // {   
        // };
    }
}