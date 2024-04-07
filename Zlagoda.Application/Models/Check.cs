namespace Zlagoda.Application.Models;

public class Check
{
    public required Guid IdCheck { get; init; }
    public required Guid IdEmployee { get; set; } 
    public Guid? IdCardCustomer { get; set; } 
    public required DateTime PrintDate { get; set; }
    public required decimal SumTotal { get; set; }
    public required decimal Vat { get; set; }
}