namespace Zlagoda.Contracts.QueryParameters;

public class EmployeeQueryParameters
{
    public bool CashiersOnly { get; set; }
    public string? SortBy { get; set; }
    public string? StartSurname { get; set; }
    public bool InCheck { get; set; }
}