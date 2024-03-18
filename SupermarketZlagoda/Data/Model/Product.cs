namespace SupermarketZlagoda.Data.Model;

public record Product(int IdProduct, int CategoryNumber, string ProductName, string Characteristics)
{
    public int CategoryNumber { get; set; } = CategoryNumber;
    public string ProductName { get; set; } = ProductName;
    public string Characteristics { get; set; } = Characteristics;
}