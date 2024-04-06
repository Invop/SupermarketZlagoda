using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SaleRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Sale sale)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var command = new SqlCommand(
            $@"INSERT INTO Sales (UPC, check_number, product_number, selling_price)" +
            "VALUES (@Upc, @CheckNumber, @ProductNumber, @SellingPrice)",
            connection);
        command.Parameters.AddWithValue("@Upc", sale.UPC);
        command.Parameters.AddWithValue("@CheckNumber", sale.CheckNumber);
        command.Parameters.AddWithValue("@ProductNumber", sale.ProductNumber);
        command.Parameters.AddWithValue("@SellingPrice", sale.SellingPrice);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }
    
    public async Task<Sale?> GetByUPCCheckAsync(string upc, Guid check)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Sales WHERE UPC = @Upc AND check_number = @CheckNumber";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Upc", upc);
        command.Parameters.AddWithValue("@CheckNumber", check);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var sale = new Sale
            {
                UPC = reader.GetString(reader.GetOrdinal("Upc")),
                CheckNumber = reader.GetGuid(reader.GetOrdinal("check_number")),
                ProductNumber = reader.GetInt32(reader.GetOrdinal("product_number")),
                SellingPrice = reader.GetDecimal(reader.GetOrdinal("selling_price")),
            };
            return sale;
        }
        return null;
    }

    public async Task<IEnumerable<Sale>> GetSaleByIdCheckAsync(Guid check)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Sales WHERE check_number = @CheckNumber";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@CheckNumber", check);
        using var reader = await command.ExecuteReaderAsync();
        var checks = new List<Sale>();
        while (await reader.ReadAsync())
        {
            var sale = new Sale
            {
                UPC = reader.GetString(reader.GetOrdinal("Upc")),
                CheckNumber = reader.GetGuid(reader.GetOrdinal("check_number")),
                ProductNumber = reader.GetInt32(reader.GetOrdinal("product_number")),
                SellingPrice = reader.GetDecimal(reader.GetOrdinal("selling_price")),
            };
            checks.Add(sale);
        }
        return checks;
    }
    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Sales";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        var checks = new List<Sale>();
        while (await reader.ReadAsync())
        {
            var sale = new Sale
            {
                UPC = reader.GetString(reader.GetOrdinal("Upc")),
                CheckNumber = reader.GetGuid(reader.GetOrdinal("check_number")),
                ProductNumber = reader.GetInt32(reader.GetOrdinal("product_number")),
                SellingPrice = reader.GetDecimal(reader.GetOrdinal("selling_price")),
            };
            checks.Add(sale);
        }
        return checks;
    }
    
    public async Task<bool> DeleteByUPCCheckAsync(Guid check)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $@"DELETE FROM Sales WHERE check_number = @CheckNumber";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@CheckNumber", check);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }
    
}