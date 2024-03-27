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

    public static ProductResponse MapToProductResponse(this Product movie)
    {
        return new ProductResponse()
        {
            Id = movie.Id,
            Name = movie.Name,
            CategoryId = movie.CategoryId,
            Characteristics = movie.Characteristics
        };
    }
    
    public static ProductsResponse MapToProductResponse(this IEnumerable<Product> movies)
    {
        return new ProductsResponse
        {
            Items = movies.Select(MapToProductResponse)
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
    
    
    public static StoreProduct MapToStoreProduct(this CreateStoreProductRequest request)
    {
        return new StoreProduct
        {
            Upc = request.Upc,
            UpcProm = request.UpcProm,
            ProductId = request.ProductId,
            Price = request.Price,
            Quantity = request.Quantity,
            IsPromotional = request.IsPromotional
        };
    }

    public static StoreProductResponse MapToStoreProductResponse(this StoreProduct request)
    {
        return new StoreProductResponse
        {
            Upc = request.Upc,
            UpcProm = request.UpcProm,
            ProductId = request.ProductId,
            Price = request.Price,
            Quantity = request.Quantity,
            IsPromotional = request.IsPromotional
        };
    }
    
    public static StoreProductsResponse MapToStoreProductResponse(this IEnumerable<StoreProduct> storeProducts)
    {   
        return new StoreProductsResponse
        {
            Items = storeProducts.Select(MapToStoreProductResponse)
        };
    }

    public static StoreProduct MapToStoreProduct(this UpdateStoreProductRequest request)
    {   
        return new StoreProduct
        {
            Upc = request.Upc,
            UpcProm = request.UpcProm,
            ProductId = request.ProductId,
            Price =request.Price,
            Quantity = request.Quantity,
            IsPromotional = request.IsPromotional
        };
    }
}