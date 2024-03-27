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
    
    public static CustomerCard MapToCustomerCard(this CreateCustomerCardRequest request)
    {
        return new CustomerCard
        {
            Id = Guid.NewGuid(),
            Surname = request.Surname,
            Name = request.Name,
            Patronymic = request.Patronymic,
            Phone = request.Phone,
            City = request.City,
            Street = request.Street,
            Index = request.Index,
            Percentage = request.Percentage
        };
    }

    public static CustomerCardResponse MapToResponse(this CustomerCard customerCard)
    {
        return new CustomerCardResponse()
        {
            Id = customerCard.Id,
            Surname = customerCard.Surname,
            Name = customerCard.Name,
            Patronymic = customerCard.Patronymic,
            Phone = customerCard.Phone,
            City = customerCard.City,
            Street = customerCard.Street,
            Index = customerCard.Index,
            Percentage = customerCard.Percentage
        };
    }
    
    public static CustomerCardsResponse MapToResponse(this IEnumerable<CustomerCard> customerCards)
    {
        return new CustomerCardsResponse
        {
            Items = customerCards.Select(MapToResponse)
        };
    }

    public static CustomerCard MapToCustomerCard(this UpdateCustomerCardRequest request,Guid id)
    {
        return new CustomerCard
        {   
            Id = id,
            Surname = request.Surname,
            Name = request.Name,
            Patronymic = request.Patronymic,
            Phone = request.Phone,
            City = request.City,
            Street = request.Street,
            Index = request.Index,
            Percentage = request.Percentage
        };
    }
}