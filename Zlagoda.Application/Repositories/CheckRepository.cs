using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public class CheckRepository : ICheckRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CheckRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Check check)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var command = new SqlCommand(
            $@"INSERT INTO Checks (check_number, id_employee, card_number, print_date, sum_total, vat)" +
            "VALUES (@IdCheck, @IdEmployee, @IdCardCustomer, @PrintDate, @SumTotal, @Vat)",
            connection);
        command.Parameters.AddWithValue("@IdCheck", check.IdCheck);
        command.Parameters.AddWithValue("@IdEmployee", check.IdEmployee);
        if (check.IdCardCustomer == null)
            command.Parameters.AddWithValue("@IdCardCustomer", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@IdCardCustomer", check.IdCardCustomer);
        }
        command.Parameters.AddWithValue("@PrintDate", check.PrintDate);
        command.Parameters.AddWithValue("@SumTotal", check.SumTotal);
        command.Parameters.AddWithValue("@Vat", check.Vat);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }
    
    public async Task<Check?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $"SELECT * FROM Checks WHERE check_number = {id}";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var check = new Check
            {
                IdCheck = reader.GetGuid(reader.GetOrdinal("check_number")),
                IdEmployee = reader.GetGuid(reader.GetOrdinal("id_employee")),
                PrintDate = reader.GetDateTime(reader.GetOrdinal("print_date")),
                SumTotal = reader.GetDecimal(reader.GetOrdinal("sum_total")),
                Vat = reader.GetDecimal(reader.GetOrdinal("vat")),
            };
            if (!reader.IsDBNull(2))
                check.IdCardCustomer = reader.GetGuid(reader.GetOrdinal("card_number"));
            return check;
        }
        return null;
    }
    public async Task<IEnumerable<Check>> GetAllAsync()
    {
        
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Checks";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        var checks = new List<Check>();
        while (await reader.ReadAsync())
        {
            var check = new Check
            {
                IdCheck = reader.GetGuid(reader.GetOrdinal("check_number")),
                IdEmployee = reader.GetGuid(reader.GetOrdinal("id_employee")),
                PrintDate = reader.GetDateTime(reader.GetOrdinal("print_date")),
                SumTotal = reader.GetDecimal(reader.GetOrdinal("sum_total")),
                Vat = reader.GetDecimal(reader.GetOrdinal("vat")),
            };
            if (!reader.IsDBNull(2))
                check.IdCardCustomer = reader.GetGuid(reader.GetOrdinal("card_number"));
            checks.Add(check);
        }
        return checks;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $@"DELETE FROM Checks WHERE check_number = @IdCheck";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@IdCheck", id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = @$"SELECT COUNT(*) FROM Checks WHERE check_number = @IdCheck";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@IdCheck", id);
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
}