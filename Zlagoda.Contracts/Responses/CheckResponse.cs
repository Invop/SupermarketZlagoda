namespace Zlagoda.Contracts.Responses;

public class CheckResponse
{
    public required Guid IdCheck { get; init; }
    public required Guid IdEmployee { get; init; }
    public Guid? IdCardCustomer { get; init; }
    public required DateTime PrintDate { get; init; }
    public required decimal SumTotal { get; init; }
    public required decimal Vat { get; init; }
}