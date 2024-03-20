﻿using Microsoft.Extensions.DependencyInjection;
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