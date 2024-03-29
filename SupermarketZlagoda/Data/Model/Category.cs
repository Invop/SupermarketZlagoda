namespace SupermarketZlagoda.Data.Model;

public record Category
{
    public Guid Id { get; init; }
    public string Name { get; set; }
}