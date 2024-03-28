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
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new SqlServerConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}