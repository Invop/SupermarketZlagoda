namespace Zlagoda.Contracts.Responses;

public class CategoryResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public int CountStoreProducts { get; set; }
    public int CountPromoProducts { get; set; }
}