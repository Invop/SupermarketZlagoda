using Microsoft.Extensions.DependencyInjection;
using Zlagoda.Application.Repositories;

namespace Zlagoda.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, ProductRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,string connectionString)
    {
        // services.AddSingleton<IDbConnectionFactory>(_ =>
        //     new NpgsqlConnectionFactory(connectionString));
        // services.AddSingleton<DbInitializer>();
        return services;
    }
}