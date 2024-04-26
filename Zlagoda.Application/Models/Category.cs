namespace Zlagoda.Application.Models;

public class Category
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public int CountStoreProducts { get; set; }
    public int CountPromoProducts { get; set; }
}