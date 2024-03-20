using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ProductRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<bool> CreateAsync(Product product)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $@"INSERT INTO Products (id_product, product_name, category_number,characteristics)
             VALUES ('{product.Id}', '{product.Name}', '{product.CategoryId}','{product.Characteristics}')";
        using var command = new SqlCommand(commandText, connection);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }


    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
}