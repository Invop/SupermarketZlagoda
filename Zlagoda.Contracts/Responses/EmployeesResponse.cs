namespace Zlagoda.Contracts.Responses;

public class EmployeesResponse
{
    public required IEnumerable<EmployeeResponse> Items { get; init; }
        = Enumerable.Empty<EmployeeResponse>();
}