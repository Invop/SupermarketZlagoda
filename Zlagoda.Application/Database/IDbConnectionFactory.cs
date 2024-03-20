using Microsoft.Data.SqlClient;

namespace Zlagoda.Application.Database;

public interface IDbConnectionFactory
{ 
    Task<SqlConnection> CreateConnectionAsync();
}

public class SqlServerConnectionFactory : IDbConnectionFactory
{   
        private readonly string _connectionString;

        public SqlServerConnectionFactory(string connectionString)
    {
            _connectionString = connectionString;
        }

        public async Task<SqlConnection> CreateConnectionAsync()
    {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
}