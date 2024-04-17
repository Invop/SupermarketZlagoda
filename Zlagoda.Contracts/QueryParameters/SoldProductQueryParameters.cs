namespace Zlagoda.Contracts.QueryParameters;

public class SoldProductQueryParameters
{
    public Guid? CashierId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}