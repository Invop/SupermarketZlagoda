using Microsoft.Data.Sqlite;
namespace SupermarketZlagoda.Data;

public class SqliteDataAccess
{
    private const string ConnectionString = "Data Source=Data\\Zlagoda.db;Foreign Keys=True;";

    public static void TestConnection()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = new SqliteCommand();
        command.Connection = connection;
        command.CommandText = "CREATE TABLE IF NOT EXISTS User (user_id INTEGER PRIMARY KEY,login TEXT NOT NULL,hash_password TEXT NOT NULL,role_type TEXT CHECK(role_type IN ('Cashier', 'Manager')) NOT NULL);";
        command.ExecuteNonQuery();
    }
}