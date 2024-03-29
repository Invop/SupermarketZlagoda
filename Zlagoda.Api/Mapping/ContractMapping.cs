using Zlagoda.Application.Models;
using Zlagoda.Contracts.Requests;
using Zlagoda.Contracts.Responses;

namespace Zlagoda.Api.Mapping;

public static class ContractMapping
{
    #region Product
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
    #endregion

    #region StoreProduct

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


    #endregion

    #region Category

    public static Category MapToCategory(this CreateCategoryRequest request)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };
    }

    public static CategoryResponse MapToResponse(this Category category)
    {
        return new CategoryResponse()
        {
            Id = category.Id,
            Name = category.Name
        };
    }
    
    public static CategoriesResponse MapToResponse(this IEnumerable<Category> movies)
    {
        return new CategoriesResponse
        {
            Items = movies.Select(MapToResponse)
        };
    }

    public static Category MapToCategory(this UpdateCategoryRequest request, Guid id)
    {
        return new Category
        {   
            Id = id,
            Name = request.Name
        };
    }
    #endregion
    
    #region Employee
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

    public static EmployeeResponse MapToResponse(this Employee em)
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
    
    public static EmployeesResponse MapToResponse(this IEnumerable<Employee> employees)
    {
        return new EmployeesResponse
        {
            Items = employees.Select(MapToResponse)
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
    #endregion
    
    #region CustomerCard
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
    #endregion
}