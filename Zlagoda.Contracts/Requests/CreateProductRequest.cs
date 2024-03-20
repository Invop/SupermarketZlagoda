namespace Zlagoda.Contracts.Requests;

public class CreateProductRequest
{
    public required string Name { get; init; }
    public required int CategoryId { get; init; }
    public required string Characteristics { get; init; }
}