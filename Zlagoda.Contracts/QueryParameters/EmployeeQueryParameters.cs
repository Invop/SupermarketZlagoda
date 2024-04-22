namespace Zlagoda.Contracts.QueryParameters;

public class EmployeeQueryParameters
{
    public bool CashiersOnly { get; set; }
    public string? SortBy { get; set; }
    public string? StartSurname { get; set; }
    public string? UserLogin { get; set; }
    public string? UserPassword { get; set; }
    public bool InCheck { get; set; }
}