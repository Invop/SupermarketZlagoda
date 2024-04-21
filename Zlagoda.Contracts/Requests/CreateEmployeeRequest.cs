namespace Zlagoda.Contracts.Requests;

public class CreateEmployeeRequest
{
    public required string Surname { get; init; }
    public required string Name { get; init; }
    public string? Patronymic { get; init; }
    public required string Role { get; init; }
    public required decimal Salary { get; init; }
    public required DateOnly DateOfStart { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string PhoneNumber { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string ZipCode { get; init; }
    public required string UserLogin { get; set; }
    public required string UserPassword { get; set; }
}