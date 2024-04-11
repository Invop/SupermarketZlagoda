using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;
using Zlagoda.Application.Services;

namespace Zlagoda.Application.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly Lazy<IStoreProductService> _storeProductService;
    public ProductRepository(IDbConnectionFactory dbConnectionFactory, Lazy<IStoreProductService> storeProductService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _storeProductService = storeProductService;
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
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Products WHERE id_product = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = reader.GetGuid(reader.GetOrdinal("id_product")),
                Name = reader.GetString(reader.GetOrdinal("product_name")),
                CategoryId = reader.GetGuid(reader.GetOrdinal("category_number")),
                Characteristics = reader.GetString(reader.GetOrdinal("characteristics"))
            };
            return product;
        }
        return null;
    }


    public async Task<IEnumerable<Product>> GetAllSortedAscendingAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Products ORDER BY product_name ASC";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        var products = new List<Product>();
        while (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = reader.GetGuid(reader.GetOrdinal("id_product")),
                Name = reader.GetString(reader.GetOrdinal("product_name")),
                CategoryId = reader.GetGuid(reader.GetOrdinal("category_number")),
                Characteristics = reader.GetString(reader.GetOrdinal("characteristics"))
            };
            products.Add(product);
        }
        return products;
    }
    
    public async Task<IEnumerable<Product>> GetAllSortedDescendingAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Products ORDER BY product_name DESC ";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        var products = new List<Product>();
        while (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = reader.GetGuid(reader.GetOrdinal("id_product")),
                Name = reader.GetString(reader.GetOrdinal("product_name")),
                CategoryId = reader.GetGuid(reader.GetOrdinal("category_number")),
                Characteristics = reader.GetString(reader.GetOrdinal("characteristics"))
            };
            products.Add(product);
        }
        return products;
    }

    public async Task<IEnumerable<Product>> GetAllUnusedAsync()
    {
        var allStoreItems = await _storeProductService.Value.GetAllAsync();
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var unusedProductIds = allStoreItems.Select(item => item.ProductId).Distinct();
        var productIds = unusedProductIds.ToList();
        if (productIds.Count == 0)
        {
            return await GetAllSortedAscendingAsync();
        }
        string idsString = string.Join(",", productIds.Select(id => $"'{id}'"));
        var commandText = $"SELECT * FROM Products WHERE id_product NOT IN ({idsString})";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        var products = new List<Product>();
        while (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = reader.GetGuid(reader.GetOrdinal("id_product")),
                Name = reader.GetString(reader.GetOrdinal("product_name")),
                CategoryId = reader.GetGuid(reader.GetOrdinal("category_number")),
                Characteristics = reader.GetString(reader.GetOrdinal("characteristics"))
            };
            products.Add(product);
        }
        return products;
        
    }

    public async Task<IEnumerable<Product>> GetAllUnusedAndCurrentAsync(Guid id)
    {
        var currentProduct = await GetByIdAsync(id);
        var allUnusedProducts = await GetAllUnusedAsync();

        if (currentProduct != null)
        {
            allUnusedProducts = allUnusedProducts.Append(currentProduct);
        }

        return allUnusedProducts;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText =
            "UPDATE Products SET product_name = @Name, category_number = @CategoryId, characteristics = @Characteristics WHERE id_product = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
        command.Parameters.AddWithValue("@Characteristics", product.Characteristics);
        command.Parameters.AddWithValue("@Id", product.Id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "DELETE FROM Products WHERE id_product = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT COUNT(*) FROM Products WHERE id_product = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
    
}