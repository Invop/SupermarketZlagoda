namespace SupermarketZlagoda.Data.Model;

public record Check
{
    public Guid IdCheck { get; init; }
    public Guid IdEmployee { get; set; } 
    public Guid? IdCardCustomer { get; set; } 
    public DateTime PrintDate { get; set; }
    public decimal SumTotal { get; set; }
    public decimal Vat { get; set; }
}