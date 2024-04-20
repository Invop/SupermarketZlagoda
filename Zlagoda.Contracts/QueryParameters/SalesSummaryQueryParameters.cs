namespace Zlagoda.Contracts.QueryParameters;

public class SalesSummaryQueryParameters
{
    public Guid? EmployId { get; init; }
    public Guid? ProductId { get; init; }
    public DateTime? PeriodFrom { get; init; }
    public DateTime? PeriodTo { get; init; }
}