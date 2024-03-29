namespace Zlagoda.Contracts.Requests;

public class UpdateProductRequest
{
    public required string Name { get; init; }
    public required Guid CategoryId { get; init; }
    public required string Characteristics { get; init; }
}