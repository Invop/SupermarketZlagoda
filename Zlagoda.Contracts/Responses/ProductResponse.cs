namespace Zlagoda.Contracts.Responses;

public class ProductResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required int CategoryId { get; init; }
    public required string Characteristics { get; init; }
}