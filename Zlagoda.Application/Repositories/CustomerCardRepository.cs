using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public class CustomerCardRepository : ICustomerCardRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CustomerCardRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<bool> CreateAsync(CustomerCard customerCard)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var command = new SqlCommand(
            $@"INSERT INTO Customer_Cards (card_number, cust_surname, cust_name, cust_patronymic, phone_number, city, street, zip_code, [percent])" +
            "VALUES (@Id, @Surname, @Name, @Patronymic, @Phone, @City, @Street, @Index, @Percentage)",
            connection);
        command.Parameters.AddWithValue("@Id", customerCard.Id);
        command.Parameters.AddWithValue("@Surname", customerCard.Surname);
        command.Parameters.AddWithValue("@Name", customerCard.Name);
        if(string.IsNullOrEmpty(customerCard.Patronymic))
            command.Parameters.AddWithValue("@Patronymic", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@Patronymic", customerCard.Patronymic);
        }
        command.Parameters.AddWithValue("@Phone", customerCard.Phone);
        if(string.IsNullOrEmpty(customerCard.City))
            command.Parameters.AddWithValue("@City", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@City", customerCard.City);
        }
        if(string.IsNullOrEmpty(customerCard.Street))
            command.Parameters.AddWithValue("@Street", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@Street", customerCard.Street);
        }
        if(string.IsNullOrEmpty(customerCard.Index))
            command.Parameters.AddWithValue("@Index", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@Index", customerCard.Index);
        }

        command.Parameters.AddWithValue("@Percentage", customerCard.Percentage);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<CustomerCard?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Customer_Cards WHERE card_number = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var customerCard = new CustomerCard
            {
                Id = reader.GetGuid(reader.GetOrdinal("card_number")),
                Surname = reader.GetString(reader.GetOrdinal("cust_surname")),
                Name = reader.GetString(reader.GetOrdinal("cust_name")),
                Phone = reader.GetString(reader.GetOrdinal("phone_number")),
                Percentage = reader.GetInt32(reader.GetOrdinal("percent"))
            };
            if (!reader.IsDBNull(3))
                customerCard.Patronymic = reader.GetString(reader.GetOrdinal("cust_patronymic"));
            if (!reader.IsDBNull(5))
                customerCard.City = reader.GetString(reader.GetOrdinal("city"));
            if (!reader.IsDBNull(6))
                customerCard.Street = reader.GetString(reader.GetOrdinal("street"));
            if (!reader.IsDBNull(7))
                customerCard.Index = reader.GetString(reader.GetOrdinal("zip_code"));
            return customerCard;
        }
        return null;
    }

    private SqlCommand GetCommandWithParameters(CustomerCardQueryParameters parameters, SqlCommand command)
    {
        if (parameters.Percentage != null)
        {
            command.Parameters.AddWithValue("@Percentage", parameters.Percentage);
        }
        if (!string.IsNullOrWhiteSpace(parameters.StartSurname))
        {
            command.Parameters.AddWithValue("@StartSurname", parameters.StartSurname);
        }
    
        return command;
    }

    private void AppendAdditionalCriteria(StringBuilder commandText, CustomerCardQueryParameters? parameters)
    {
        if (parameters == null) return;
        if (!string.IsNullOrWhiteSpace(parameters.StartSurname))
        {
            commandText.Append(" AND (cust_surname LIKE @StartSurname + '%' OR cust_surname = @StartSurname)");
        }
        if (parameters.Percentage != null)
        {
            commandText.Append(" AND [percent] = @Percentage");
        }
        if (!string.IsNullOrEmpty(parameters.SortBy))
        {
            var sortColumns = parameters.SortBy.Split(','); 
            commandText.Append(" ORDER BY ");
            for (int i = 0; i < sortColumns.Length; i++)
            {
                commandText.Append(sortColumns[i]);
                if (!string.IsNullOrEmpty(parameters.SortOrder))
                {
                    commandText.Append($" {parameters.SortOrder}");
                }
                if (i < sortColumns.Length - 1)
                {
                    commandText.Append(", "); 
                }
            }
        }
    }
    
    public async Task<IEnumerable<CustomerCard>> GetAllAsync(CustomerCardQueryParameters? parameters)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = new StringBuilder(@"SELECT * FROM Customer_Cards
                                                WHERE 1=1");

        AppendAdditionalCriteria(commandText, parameters);

        await using var command = new SqlCommand(commandText.ToString(), connection);
        if(parameters != null)
        {
            GetCommandWithParameters(parameters, command);
        }

        await using var reader = await command.ExecuteReaderAsync();
        var customerCards = new List<CustomerCard>();
        while (await reader.ReadAsync())
        {
            var customerCard = new CustomerCard
            {
                Id = reader.GetGuid(reader.GetOrdinal("card_number")),
                Surname = reader.GetString(reader.GetOrdinal("cust_surname")),
                Name = reader.GetString(reader.GetOrdinal("cust_name")),
                Phone = reader.GetString(reader.GetOrdinal("phone_number")),
                Percentage = reader.GetInt32(reader.GetOrdinal("percent"))
            };
            if (!reader.IsDBNull(3))
                customerCard.Patronymic = reader.GetString(reader.GetOrdinal("cust_patronymic"));
            if (!reader.IsDBNull(5))
                customerCard.City = reader.GetString(reader.GetOrdinal("city"));
            if (!reader.IsDBNull(6))
                customerCard.Street = reader.GetString(reader.GetOrdinal("street"));
            if (!reader.IsDBNull(7))
                customerCard.Index = reader.GetString(reader.GetOrdinal("zip_code"));
            customerCards.Add(customerCard);
        }
        return customerCards;
    }



    public async Task<bool> UpdateAsync(CustomerCard customerCard)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText =
            $@"UPDATE Customer_Cards SET cust_surname = @Surname, cust_name = @Name, cust_patronymic = @Patronymic, phone_number = @Phone, 
                          city = @City, street = @Street, zip_code = @Index, [percent] = @Percentage WHERE card_number = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", customerCard.Id);
        command.Parameters.AddWithValue("@Surname", customerCard.Surname);
        command.Parameters.AddWithValue("@Name", customerCard.Name);
        if(string.IsNullOrEmpty(customerCard.Patronymic))
            command.Parameters.AddWithValue("@Patronymic", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@Patronymic", customerCard.Patronymic);
        }
        command.Parameters.AddWithValue("@Phone", customerCard.Phone);
        if(string.IsNullOrEmpty(customerCard.City))
            command.Parameters.AddWithValue("@City", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@City", customerCard.City);
        }
        if(string.IsNullOrEmpty(customerCard.Street))
            command.Parameters.AddWithValue("@Street", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@Street", customerCard.Street);
        }
        if(string.IsNullOrEmpty(customerCard.Index))
            command.Parameters.AddWithValue("@Index", DBNull.Value);
        else
        {
            command.Parameters.AddWithValue("@Index", customerCard.Index);
        }

        command.Parameters.AddWithValue("@Percentage", customerCard.Percentage);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "DELETE FROM Customer_Cards WHERE card_number = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT COUNT(*) FROM Customer_Cards WHERE card_number = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
    
}