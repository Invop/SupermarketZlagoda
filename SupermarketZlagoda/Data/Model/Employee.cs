namespace SupermarketZlagoda.Data.Model;

public record Employee
{
    public string IdEmployee { get; }
    public string EmployeeSurname { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeePatronymic { get; set; }
    public string Role { get; set; }
    public decimal Salary { get; set; }
    public DateOnly DateOfStart { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
}