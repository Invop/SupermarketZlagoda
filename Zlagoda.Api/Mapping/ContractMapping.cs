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
    
    public static StoreProductResponse MapToStoreProductResponse(this StoreProduct movie)
    {
        return new StoreProductResponse()
        {
            Upc = movie.Upc,
            UpcProm = movie.UpcProm,
            ProductId = movie.ProductId,
            Price = movie.Price,
            Quantity = movie.Quantity,
            IsPromotional = movie.IsPromotional
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
            Price = request.Price,
            Quantity = request.Quantity,
            IsPromotional = request.IsPromotional
        };
    }
    
    
    
    public static Employee MapToEmployee(this CreateEmployeeRequest request)
    {
        return new Employee
        {
            Id = Guid.NewGuid(),
            Surname = request.Surname,
            Name = request.Name,
            Patronymic = request.Patronymic,
            Role = request.Role,
            Salary = request.Salary,
            DateOfBirth = request.DateOfBirth,
            DateOfStart = request.DateOfStart,
            PhoneNumber = request.PhoneNumber,
            City = request.City,
            Street = request.Street,
            ZipCode = request.ZipCode
        };
    }

    public static EmployeeResponse MapToProductResponse(this Employee em)
    {
        return new EmployeeResponse()
        {
            Id = em.Id,
            Surname = em.Surname,
            Name = em.Name,
            Patronymic = em.Patronymic,
            Role = em.Role,
            Salary = em.Salary,
            DateOfBirth = em.DateOfBirth,
            DateOfStart = em.DateOfStart,
            PhoneNumber = em.PhoneNumber,
            City = em.City,
            Street = em.Street,
            ZipCode = em.ZipCode
        };
    }
    
    public static EmployeesResponse MapToProductResponse(this IEnumerable<Employee> employees)
    {
        return new EmployeesResponse
        {
            Items = employees.Select(MapToProductResponse)
        };
    }

    public static Employee MapToEmployee(this UpdateEmployeeRequest request, Guid id)
    {
        return new Employee
        {   
            Id = id,
            Surname = request.Surname,
            Name = request.Name,
            Patronymic = request.Patronymic,
            Role = request.Role,
            Salary = request.Salary,
            DateOfBirth = request.DateOfBirth,
            DateOfStart = request.DateOfStart,
            PhoneNumber = request.PhoneNumber,
            City = request.City,
            Street = request.Street,
            ZipCode = request.ZipCode
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

    public static CustomerCardResponse MapToProductResponse(this CustomerCard customerCard)
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
    
    public static CustomerCardsResponse MapToProductResponse(this IEnumerable<CustomerCard> customerCards)
    {
        return new CustomerCardsResponse
        {
            Items = customerCards.Select(MapToProductResponse)
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