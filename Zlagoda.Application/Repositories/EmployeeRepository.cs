using System.Text;
using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public EmployeeRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Employee em)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText =
            $@"INSERT INTO Employees (id_employee, empl_surname, empl_name, empl_patronymic, empl_role,
                       salary, date_of_birth, date_of_start, phone_number, city, street, zip_code, user_login, user_password)
             VALUES (@Id, @Name, @Surname, @Patronymic, @Role, @Salary, @DateOfBirth,
                     @DateOfStart, @PhoneNumber, @City, @Street, @ZipCode, @UserLogin, @UserPassword)";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", em.Id);
        command.Parameters.AddWithValue("@Surname", em.Surname);
        command.Parameters.AddWithValue("@Name", em.Name);
        if (string.IsNullOrEmpty(em.Patronymic))
            command.Parameters.AddWithValue("@Patronymic", DBNull.Value);
        else
            command.Parameters.AddWithValue("@Patronymic", em.Patronymic);
        command.Parameters.AddWithValue("@Role", em.Role);
        command.Parameters.AddWithValue("@Salary", em.Salary);
        command.Parameters.AddWithValue("@DateOfBirth", em.DateOfBirth);
        command.Parameters.AddWithValue("@DateOfStart", em.DateOfStart);
        command.Parameters.AddWithValue("@PhoneNumber", em.PhoneNumber);
        command.Parameters.AddWithValue("@City", em.City);
        command.Parameters.AddWithValue("@Street", em.Street);
        command.Parameters.AddWithValue("@ZipCode", em.ZipCode);
        command.Parameters.AddWithValue("@UserLogin", em.UserLogin);
        command.Parameters.AddWithValue("@UserPassword", em.UserPassword);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $"SELECT * FROM Employees WHERE id_employee = {id}";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var employee = new Employee
            {
                Id = reader.GetGuid(reader.GetOrdinal("id_employee")),
                Surname = reader.GetString(reader.GetOrdinal("empl_surname")),
                Name = reader.GetString(reader.GetOrdinal("empl_name")),
                Role = reader.GetString(reader.GetOrdinal("empl_role")),
                Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                DateOfStart = reader.GetDateTime(reader.GetOrdinal("date_of_start")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                City = reader.GetString(reader.GetOrdinal("city")),
                Street = reader.GetString(reader.GetOrdinal("street")),
                ZipCode = reader.GetString(reader.GetOrdinal("zip_code")),
                UserLogin = reader.GetString(reader.GetOrdinal("user_login")),
                UserPassword = reader.GetString(reader.GetOrdinal("user_password"))
            };
            if (!reader.IsDBNull(3))
                employee.Patronymic = reader.GetString(reader.GetOrdinal("empl_patronymic"));
            return employee;
        }

        return null;
    }

    private void AppendAdditionalCriteria(StringBuilder commandText, EmployeeQueryParameters? parameters)
    {
        if (parameters == null) return;
        if (parameters.CashiersOnly)
        {
            commandText.Append(" AND empl_role = 'Cashier'");
        }

        if (!string.IsNullOrWhiteSpace(parameters.StartSurname))
        {
            commandText.Append(" AND (empl_surname LIKE @StartSurname + '%' OR empl_surname = @StartSurname)");
        }

        if (!string.IsNullOrWhiteSpace(parameters.UserLogin))
        {
            commandText.Append(" AND user_login = @UserLogin");
        }

        if (!string.IsNullOrWhiteSpace(parameters.UserPassword))
        {
            commandText.Append(" AND user_password = @UserPassword");
        }

        if (parameters.InCheck)
        {
            commandText.Append(" AND id_employee IN (SELECT id_employee FROM Checks)");
        }

        if (!string.IsNullOrEmpty(parameters.SortBy))
        {
            commandText.Append($" ORDER BY {parameters.SortBy}");
        }
    }

    private void GetCommandWithParameters(EmployeeQueryParameters parameters, SqlCommand command)
    {
        if (parameters is null) return;
        if (!string.IsNullOrWhiteSpace(parameters.StartSurname))
        {
            command.Parameters.AddWithValue("@StartSurname", parameters.StartSurname);
        }

        if (!string.IsNullOrWhiteSpace(parameters.UserLogin))
        {
            command.Parameters.AddWithValue("@UserLogin", parameters.UserLogin);
        }

        if (!string.IsNullOrWhiteSpace(parameters.UserPassword))
        {
            command.Parameters.AddWithValue("@UserPassword", parameters.UserPassword);
        }
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(EmployeeQueryParameters? parameters)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = new StringBuilder(@"SELECT * FROM Employees 
                                          WHERE 1=1");
        AppendAdditionalCriteria(commandText, parameters);
        await using var command = new SqlCommand(commandText.ToString(), connection);
        GetCommandWithParameters(parameters, command);
        await using var reader = await command.ExecuteReaderAsync();
        var employees = new List<Employee>();
        while (await reader.ReadAsync())
        {
            var employee = new Employee
            {
                Id = reader.GetGuid(reader.GetOrdinal("id_employee")),
                Surname = reader.GetString(reader.GetOrdinal("empl_surname")),
                Name = reader.GetString(reader.GetOrdinal("empl_name")),
                Role = reader.GetString(reader.GetOrdinal("empl_role")),
                Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                DateOfStart = reader.GetDateTime(reader.GetOrdinal("date_of_start")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                City = reader.GetString(reader.GetOrdinal("city")),
                Street = reader.GetString(reader.GetOrdinal("street")),
                ZipCode = reader.GetString(reader.GetOrdinal("zip_code")),
                UserLogin = reader.GetString(reader.GetOrdinal("user_login")),
                UserPassword = reader.GetString(reader.GetOrdinal("user_password"))
            };
            if (!reader.IsDBNull(3))
                employee.Patronymic = reader.GetString(reader.GetOrdinal("empl_patronymic"));
            employees.Add(employee);
        }

        return employees;
    }

    public async Task<bool> UpdateAsync(Employee em)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText =
            $@"UPDATE Employees
              SET empl_surname = @Surname, empl_name = @Name, empl_patronymic = @Patronymic, empl_role = @Role,
                  salary = @Salary, date_of_birth = @DateOfBirth, date_of_start = @DateOfStart,
                  phone_number = @PhoneNumber, city = @City, street = @Street, zip_code = @Code, user_login = @UserLogin, user_password = @UserPassword
              WHERE id_employee = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Surname", em.Surname);
        command.Parameters.AddWithValue("@Name", em.Name);
        if (string.IsNullOrEmpty(em.Patronymic))
            command.Parameters.AddWithValue("@Patronymic", DBNull.Value);
        else
            command.Parameters.AddWithValue("@Patronymic", em.Patronymic);
        command.Parameters.AddWithValue("@Role", em.Role);
        command.Parameters.AddWithValue("@Salary", em.Salary);
        command.Parameters.AddWithValue("@DateOfBirth", em.DateOfBirth);
        command.Parameters.AddWithValue("@DateOfStart", em.DateOfStart);
        command.Parameters.AddWithValue("@PhoneNumber", em.PhoneNumber);
        command.Parameters.AddWithValue("@City", em.City);
        command.Parameters.AddWithValue("@Street", em.Street);
        command.Parameters.AddWithValue("@Code", em.ZipCode);
        command.Parameters.AddWithValue("@Id", em.Id);
        command.Parameters.AddWithValue("@UserLogin", em.UserLogin);
        command.Parameters.AddWithValue("@UserPassword", em.UserPassword);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $"DELETE FROM Employees WHERE id_employee = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $"SELECT COUNT(*) FROM Employees WHERE id_employee = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<IEnumerable<Employee>> GetCashiersServedAllCustomers()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = new StringBuilder("""
                                            SELECT *
                                            FROM Employees AS Empl
                                            WHERE NOT EXISTS (
                                                            SELECT Cc.card_number
                                                            FROM Customer_Cards AS Cc
                                                            WHERE NOT EXISTS (
                                                                            SELECT *
                                                                            FROM Checks AS ch
                                                                            WHERE ch.id_employee = Empl.id_employee 
                                                                              AND ch.card_number = Cc.card_number
                                                                )
                                                            )
                                            """);
        await using var command = new SqlCommand(commandText.ToString(), connection);
        await using var reader = await command.ExecuteReaderAsync();
        var employees = new List<Employee>();
        while (await reader.ReadAsync())
        {
            var employee = new Employee
            {
                Id = reader.GetGuid(reader.GetOrdinal("id_employee")),
                Surname = reader.GetString(reader.GetOrdinal("empl_surname")),
                Name = reader.GetString(reader.GetOrdinal("empl_name")),
                Role = reader.GetString(reader.GetOrdinal("empl_role")),
                Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                DateOfStart = reader.GetDateTime(reader.GetOrdinal("date_of_start")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                City = reader.GetString(reader.GetOrdinal("city")),
                Street = reader.GetString(reader.GetOrdinal("street")),
                ZipCode = reader.GetString(reader.GetOrdinal("zip_code")),
                UserLogin = reader.GetString(reader.GetOrdinal("user_login")),
                UserPassword = reader.GetString(reader.GetOrdinal("user_password"))
            };
            if (!reader.IsDBNull(3))
                employee.Patronymic = reader.GetString(reader.GetOrdinal("empl_patronymic"));
            employees.Add(employee);
        }

        return employees;
    }
}