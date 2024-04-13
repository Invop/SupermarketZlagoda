using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.QueryParameters;

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


    private SqlCommand GetCommandWithParameters(ProductQueryParameters parameters, SqlCommand command)
    {
        if (parameters.Category != null)
        {
            var categoryParameters = parameters.Category.Select((id, index) =>
                new SqlParameter($"@Category{index}", SqlDbType.UniqueIdentifier) { Value = id }).ToArray();
            command.Parameters.AddRange(categoryParameters);
        }
        if (!string.IsNullOrWhiteSpace(parameters.ProductTitleMatch))
        {
            command.Parameters.AddWithValue("@ProductTitleMatch", $"{parameters.ProductTitleMatch}");
        }
        return command;
    }

    private void AppendAdditionalCriteria(StringBuilder commandText, ProductQueryParameters? parameters)
    {
        if (parameters == null) return;

        if (parameters.Category != null && parameters.Category.Any())
        {
            var ids = parameters.Category.Select((id, index) => $"@Category{index}").ToArray();
            commandText.Append($" AND category_number IN ({string.Join(",", ids)})");
        }
        if (!string.IsNullOrWhiteSpace(parameters.ProductTitleMatch))
        {
            commandText.Append(" AND (product_name LIKE '%'+ @ProductTitleMatch +'%' OR product_name = @ProductTitleMatch)");
        }
        if (!string.IsNullOrEmpty(parameters.SortBy))
        {
            commandText.Append($" ORDER BY {parameters.SortBy}");
            if (!string.IsNullOrEmpty(parameters.SortOrder))
            {
                commandText.Append($" {parameters.SortOrder}");
            }
        }

    }

    public async Task<IEnumerable<Product>> GetAllAsync(ProductQueryParameters? parameters)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = new StringBuilder(@"SELECT * FROM Products WHERE 1=1");

        AppendAdditionalCriteria(commandText, parameters);
        using var command = new SqlCommand(commandText.ToString(), connection);
        if(parameters != null)
        {
            GetCommandWithParameters(parameters, command);
        }

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
        var allStoreItems = await _storeProductService.Value.GetAllAsync(null);
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var unusedProductIds = allStoreItems.Select(item => item.ProductId).Distinct();
        var productIds = unusedProductIds.ToList();
         if (productIds.Count == 0)
         {
             return await GetAllAsync(null);
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