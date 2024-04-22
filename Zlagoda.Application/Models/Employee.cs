namespace Zlagoda.Application.Models;

public class Employee
{
    public required Guid Id { get; init; }
    public required string Surname { get; set; }
    public required string Name { get; set; }
    public string? Patronymic { get; set; }
    public required string Role { get; set; }
    public required decimal Salary { get; set; }
    public required DateTime DateOfStart { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string PhoneNumber { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string ZipCode { get; set; }
    public required string UserLogin { get; set; }
    public required string UserPassword { get; set; }
}