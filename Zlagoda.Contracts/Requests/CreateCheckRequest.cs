namespace Zlagoda.Contracts.Requests;

public class CreateCheckRequest
{
    public required Guid IdEmployee { get; init; }
    public Guid? IdCardCustomer { get; init; }
    public required DateTime PrintDate { get; init; }
    public required decimal SumTotal { get; init; }
    public required decimal Vat { get; init; }
}