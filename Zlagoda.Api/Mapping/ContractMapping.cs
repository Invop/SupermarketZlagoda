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
}