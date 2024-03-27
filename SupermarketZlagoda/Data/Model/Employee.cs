namespace SupermarketZlagoda.Data.Model;

public record Employee(int IdEmployee,
    string EmployeeSurname, string EmployeeName, string EmployeePatronymic, string Role,
    decimal Salary, DateOnly DateOfStart, DateOnly DateOfBirth, string PhoneNumber, string City,
    string Street, string ZipCode)
{
    public string EmployeeSurname { get; set; } = EmployeeSurname;
    public string EmployeeName { get; set; } = EmployeeName;
    public string EmployeePatronymic { get; set; } = EmployeePatronymic;
    public string Role { get; set; } = Role;
    public decimal Salary { get; set; } = Salary;
    public DateOnly DateOfStart { get; set; } = DateOfStart;
    public DateOnly DateOfBirth { get; set; } = DateOfBirth;
    public string PhoneNumber { get; set; } = PhoneNumber;
    public string City { get; set; } = City;
    public string Street { get; set; } = Street;
    public string ZipCode { get; set; } = ZipCode;
}