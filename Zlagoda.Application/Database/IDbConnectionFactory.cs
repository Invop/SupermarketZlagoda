using Microsoft.Data.SqlClient;

namespace Zlagoda.Application.Database;

public interface IDbConnectionFactory
{
    Task<SqlConnection> CreateConnectionAsync();
}

// Клас, що реалізує інтерфейс фабрики з'єднань БД
public class SqlServerConnectionFactory : IDbConnectionFactory
{
    // Рядок з'єднання
    private readonly string _connectionString;

    // Конструктор, що приймає рядок з'єднання як вхідний параметр
    public SqlServerConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Асинхронний метод створення з'єднання з базою даних
    public async Task<SqlConnection> CreateConnectionAsync()
    {
        // Створюємо нове з'єднання з базою даних
        var connection = new SqlConnection(_connectionString);
        // Відкриваємо це з'єднання
        await connection.OpenAsync();
        // Повертаємо з'єднання
        return connection;
    }
}