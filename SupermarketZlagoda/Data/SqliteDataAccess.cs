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
        command.CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Age INTEGER NOT NULL)";
        command.ExecuteNonQuery();
 
        Console.WriteLine("Table Users created");
    }
}