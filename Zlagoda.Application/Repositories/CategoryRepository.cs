using System.Text;
using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CategoryRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Category category)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $@"INSERT INTO Categories (category_number, category_name)
             VALUES ('{category.Id}', '{category.Name}')";
        using var command = new SqlCommand(commandText, connection);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $@"SELECT * FROM Categories WHERE category_number = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var category = new Category
            {
                Id = reader.GetGuid(reader.GetOrdinal("category_number")),
                Name = reader.GetString(reader.GetOrdinal("category_name"))
            };
            return category;
        }

        return null;
    }
    
    public async Task<IEnumerable<Category>> GetAllAsync(CategoryQueryParameters? parameters)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = new StringBuilder("SELECT * FROM Categories");
        AppendAdditionalCriteria(commandText, parameters);
        using var command = new SqlCommand(commandText.ToString(), connection);
        using var reader = await command.ExecuteReaderAsync();
        var categories = new List<Category>();
        while (await reader.ReadAsync())
        {
            var category = new Category
            {
                Id = reader.GetGuid(reader.GetOrdinal("category_number")),
                Name = reader.GetString(reader.GetOrdinal("category_name"))
            };
            categories.Add(category);
        }
        return categories;
    }

    private void AppendAdditionalCriteria(StringBuilder commandText, CategoryQueryParameters? parameters)
    {
        if (parameters == null) return;
        if (string.IsNullOrEmpty(parameters.SortBy)) return;
        commandText.Append($" ORDER BY {parameters.SortBy}");
        if (!string.IsNullOrEmpty(parameters.SortOrder))
        {
            commandText.Append($" {parameters.SortOrder}");
        }
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText =
            $@"UPDATE Categories SET category_name = @Name WHERE category_number = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Name", category.Name);
        command.Parameters.AddWithValue("@Id", category.Id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $@"DELETE FROM Categories WHERE category_number = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = @$"SELECT COUNT(*) FROM Categories WHERE category_number = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
}