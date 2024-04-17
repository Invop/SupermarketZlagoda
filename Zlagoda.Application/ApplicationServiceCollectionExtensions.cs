using Microsoft.Extensions.DependencyInjection;
using Zlagoda.Application.Database;
using Zlagoda.Application.Repositories;
using Zlagoda.Application.Services;

namespace Zlagoda.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IProductService, ProductService>();
        
        
        services.AddSingleton<IStoreProductRepository, StoreProductRepository>();
        services.AddSingleton<IStoreProductService, StoreProductService>();

        
        services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
        services.AddSingleton<IEmployeeService, EmployeeService>();
        
        
        services.AddSingleton<ICustomerCardRepository, CustomerCardRepository>();
        services.AddSingleton<ICustomerCardService, CustomerCardService>();
        
        
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<ICategoryService, CategoryService>();
        
        services.AddSingleton<ICheckRepository, CheckRepository>();
        services.AddSingleton<ICheckService, CheckService>();
        
        services.AddSingleton<ISaleRepository, SaleRepository>();
        services.AddSingleton<ISaleService, SaleService>();
        
        services.AddSingleton<ISoldProductRepository, SoldProductRepository>();
        services.AddSingleton<ISoldProductService, SoldProductService>();
        
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new SqlServerConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
    
    public static void AddLazy(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IStoreProductService, StoreProductService>();
        services.AddTransient<Lazy<IProductRepository>>(provider => new Lazy<IProductRepository>(provider.GetRequiredService<IProductRepository>));
        services.AddTransient<Lazy<IStoreProductService>>(provider => new Lazy<IStoreProductService>(provider.GetRequiredService<IStoreProductService>));
    }
}