namespace Zlagoda.Application.Models;

public class Employee
{
    private static long _counter = 0;
    public static string Increment() { return $"{_counter++}"; }
    public required string Id { get; init; }
    public required string Surname { get; set; }
    public required string Name { get; set; }
    public string? Patronymic { get; set; }
    public required string Role { get; set; }
    public required decimal Salary { get; set; }
    public required DateOnly DateOfStart { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string PhoneNumber { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string ZipCode { get; set; }
}