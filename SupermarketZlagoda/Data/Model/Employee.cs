namespace SupermarketZlagoda.Data.Model;


public record Employee(int IdEmployee,
    string EmployeeSurname, string EmployeeName, string EmployeePatronymic, string Role,
    decimal Salary, DateOnly DateOfStart, DateOnly DateOfBirth, string PhoneNumber, string City,
    string Street, string ZipCode);