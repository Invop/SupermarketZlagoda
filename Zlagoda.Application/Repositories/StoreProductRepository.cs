using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public class StoreProductRepository : IStoreProductRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly Lazy<IProductRepository> _productRepository;
    public StoreProductRepository(IDbConnectionFactory dbConnectionFactory, Lazy<IProductRepository>  productRepository)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _productRepository = productRepository;
    }

    private async Task<bool> IsProductExists(StoreProduct storeProduct)
    {
        var productExists = await _productRepository.Value.ExistsByIdAsync(storeProduct.ProductId);
        return productExists;
    }
    public async Task<bool> CreateAsync(StoreProduct storeProduct)
    {
        var isProductExist = await IsProductExists(storeProduct);
        if (!isProductExist)
            return false;
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var command = new SqlCommand(
            "INSERT INTO Store_Products (UPC, UPC_prom, id_product, selling_price, products_number, promotional_product) " +
            "VALUES (@UPC, @UPC_prom, @id_product, @selling_price, @products_number, @Promotional_Product)",
            connection);
        command.Parameters.AddWithValue("@UPC", storeProduct.Upc);
        if(string.IsNullOrEmpty(storeProduct.UpcProm))
            command.Parameters.AddWithValue("@UPC_prom", DBNull.Value);
        else
            command.Parameters.AddWithValue("@UPC_prom", storeProduct.UpcProm);
        command.Parameters.AddWithValue("@id_product", storeProduct.ProductId);
        command.Parameters.AddWithValue("@selling_price", storeProduct.Price);
        command.Parameters.AddWithValue("@products_number", storeProduct.Quantity);
        command.Parameters.AddWithValue("@promotional_product", storeProduct.IsPromotional);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<StoreProduct?> GetByUPCAsync(string upc)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Store_Products WHERE UPC = @Upc";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Upc", upc);
        using var reader = await command.ExecuteReaderAsync();
    
        if (await reader.ReadAsync())
        {
            var storeProduct = new StoreProduct
            {
                Upc = reader.IsDBNull(reader.GetOrdinal("UPC")) ? null : reader.GetString(reader.GetOrdinal("UPC")),
                UpcProm = reader.IsDBNull(reader.GetOrdinal("UPC_prom")) ? null : reader.GetString(reader.GetOrdinal("UPC_prom")),
                ProductId = reader.GetGuid(reader.GetOrdinal("id_product")),
                Price = reader.GetDecimal(reader.GetOrdinal("selling_price")),
                Quantity = reader.GetInt32(reader.GetOrdinal("products_number")),
                IsPromotional = reader.GetBoolean(reader.GetOrdinal("promotional_product"))
            };
            return storeProduct;
        }
        return null;
    }

    public async Task<IEnumerable<StoreProduct>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Store_Products";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        var storeProducts = new List<StoreProduct>();

        while (await reader.ReadAsync())
        {
            var storeProduct = new StoreProduct
            {
                Upc = reader.IsDBNull(reader.GetOrdinal("UPC")) ? null : reader.GetString(reader.GetOrdinal("UPC")),
                UpcProm = reader.IsDBNull(reader.GetOrdinal("UPC_prom")) ? null : reader.GetString(reader.GetOrdinal("UPC_prom")),
                ProductId = reader.GetGuid(reader.GetOrdinal("id_product")),
                Price = reader.GetDecimal(reader.GetOrdinal("selling_price")),
                Quantity = reader.GetInt32(reader.GetOrdinal("products_number")),
                IsPromotional = reader.GetBoolean(reader.GetOrdinal("promotional_product"))
            };
            storeProducts.Add(storeProduct);
        }
        return storeProducts;
    }

    public async Task<bool> UpdatePromProductsAsync(string prevUpc,string? newUpc)
    {   
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        const string queryString = "UPDATE Store_Products SET UPC_prom = @newUpc WHERE UPC_prom = @prevUpc;";
        using var command = new SqlCommand(queryString, connection);
        if (string.IsNullOrEmpty(newUpc))
            command.Parameters.AddWithValue("@newUpc", DBNull.Value);
        else
            command.Parameters.AddWithValue("@newUpc", newUpc);
        command.Parameters.AddWithValue("@prevUpc", prevUpc);
        var result = await command.ExecuteNonQueryAsync();

        return result > 0;
    }

    public async Task<bool> UpdateAsync(StoreProduct product,string prevUpc)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        const string queryString =
            "UPDATE Store_Products SET UPC = @UPC, UPC_prom = @UPC_prom, id_product = @id_product, selling_price = @selling_price, products_number = @products_number, promotional_product = @promotional_product WHERE UPC = @prevUPC";
        using var command = new SqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@UPC", product.Upc);
        command.Parameters.AddWithValue("@prevUPC", prevUpc);
        if (string.IsNullOrEmpty(product.UpcProm))
            command.Parameters.AddWithValue("@UPC_prom", DBNull.Value);
        else
            command.Parameters.AddWithValue("@UPC_prom", product.UpcProm);
        command.Parameters.AddWithValue("@id_product", product.ProductId);
        command.Parameters.AddWithValue("@selling_price", product.Price);
        command.Parameters.AddWithValue("@products_number", product.Quantity);
        command.Parameters.AddWithValue("@promotional_product", product.IsPromotional);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> DeleteByUPCAsync(string upc)
    {
        
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        const string queryString = "DELETE FROM Store_Products WHERE UPC = @Upc";
        using var command = new SqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@Upc", upc);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
        
    }

    public async Task<bool> ExistsByUPCAsync(string upc)
    {
        
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        const string queryString = "SELECT COUNT(*) FROM Store_Products WHERE UPC = @Upc";
        using var command = new SqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@Upc", upc);
        var result = await command.ExecuteScalarAsync();
        return (int)result > 0;
        
    }

    public async Task<IEnumerable<string>> GetAllNotPromoProductUPC()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        const string queryString = """
                                   
                                           SELECT UPC
                                           FROM Store_Products
                                           WHERE id_product IN (
                                               SELECT id_product
                                               FROM Store_Products
                                               GROUP BY id_product
                                               HAVING COUNT(*) = 1)
                                           AND promotional_product = 0
                                   """;
        using var command = new SqlCommand(queryString, connection);
        using var reader = await command.ExecuteReaderAsync();
        var upcs = new List<string>();
        while (await reader.ReadAsync())
        {
            var upc = reader.GetString(reader.GetOrdinal("UPC"));
            upcs.Add(upc);
        }
        return upcs;
    }
}