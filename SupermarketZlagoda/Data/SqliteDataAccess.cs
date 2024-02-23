using System.Data;
using Microsoft.Data.SqlClient;

namespace SupermarketZlagoda.Data;

public static class SqliteDataAccess
{
    public static void TestConnection()
    {
        string connectionString = "Server=localhost;"+
                                  "Database=master;"+
                                  "Integrated Security=True;" +
                                  "TrustServerCertificate=True;";

        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        Console.WriteLine("Connected successfully.");
        string dbName = "zlagoda";
        using var command = new SqlCommand($"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name='{dbName}') CREATE DATABASE {dbName}", connection);
        command.ExecuteNonQuery();
        Console.WriteLine($"Database '{dbName}' ensured. It's created if it was not existing.");
    }
}