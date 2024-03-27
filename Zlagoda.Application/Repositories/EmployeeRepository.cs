using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;

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
        var empl_patronymic = em.Patronymic is null ? "" : ", empl_patronymic";
        var patronymic = em.Patronymic is null ? "" : $", '{em.Patronymic}'";
        var commandText =
            $@"INSERT INTO Employees (id_employee, empl_surname, empl_name{empl_patronymic}, empl_role,
                       salary, date_of_birth, date_of_start, phone_number, city, street, zip_code)
             VALUES ({em.Id}, '{em.Name}', '{em.Surname}'{patronymic}, '{em.Role}', {em.Salary}, '{em.DateOfBirth}',
                     '{em.DateOfStart}', '{em.PhoneNumber}', '{em.City}', '{em.Street}', '{em.ZipCode}')";
        using var command = new SqlCommand(commandText, connection);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<Employee?> GetByIdAsync(string id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = $"SELECT * FROM Employees WHERE id_employee = {id}";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var employee = new Employee
            {
                Id = reader.GetString(reader.GetOrdinal("id_employee")),
                Surname = reader.GetString(reader.GetOrdinal("empl_surname")),
                Name = reader.GetString(reader.GetOrdinal("empl_name")),
                Role = reader.GetString(reader.GetOrdinal("empl_role")),
                Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                DateOfStart = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_start"))),
                PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                City = reader.GetString(reader.GetOrdinal("city")),
                Street = reader.GetString(reader.GetOrdinal("street")),
                ZipCode = reader.GetString(reader.GetOrdinal("zip_code"))
            };
            if (!reader.IsDBNull(3))
                employee.Patronymic = reader.GetString(reader.GetOrdinal("empl_patronymic"));
            return employee;
        }
        return null;
    }


    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT * FROM Employees";
        using var command = new SqlCommand(commandText, connection);
        using var reader = await command.ExecuteReaderAsync();
        var employees = new List<Employee>();
        while (await reader.ReadAsync())
        {
            var employee = new Employee
            {
                Id = reader.GetString(reader.GetOrdinal("id_employee")),
                Surname = reader.GetString(reader.GetOrdinal("empl_surname")),
                Name = reader.GetString(reader.GetOrdinal("empl_name")),
                Role = reader.GetString(reader.GetOrdinal("empl_role")),
                Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                DateOfStart = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_start"))),
                PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                City = reader.GetString(reader.GetOrdinal("city")),
                Street = reader.GetString(reader.GetOrdinal("street")),
                ZipCode = reader.GetString(reader.GetOrdinal("zip_code"))
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
            @"UPDATE Employees
              SET empl_surname = @Surname, empl_name = @Name, empl_role = @Role,
                  salary = @Salary, date_of_birth = @DateOfBirth, date_of_start = @DateOfStart,
                  phone_number = @PhoneNumber, city = @City, street = @Street, zip_code = @Code
              WHERE id_employee = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Surname", em.Surname);
        command.Parameters.AddWithValue("@Name", em.Name);
        command.Parameters.AddWithValue("@Role", em.Role);
        command.Parameters.AddWithValue("@Salary", em.Salary);
        command.Parameters.AddWithValue("@DateOfBirth", em.DateOfBirth);
        command.Parameters.AddWithValue("@DateOfStart", em.DateOfStart);
        command.Parameters.AddWithValue("@PhoneNumber", em.PhoneNumber);
        command.Parameters.AddWithValue("@City", em.City);
        command.Parameters.AddWithValue("@Street", em.Street);
        command.Parameters.AddWithValue("@Code", em.ZipCode);
        command.Parameters.AddWithValue("@Id", em.Id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(string id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "DELETE FROM Employees WHERE id_employee = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(string id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = "SELECT COUNT(*) FROM Employees WHERE id_product = @Id";
        using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", id);
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
}