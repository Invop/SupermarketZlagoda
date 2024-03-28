namespace Zlagoda.Contracts.Responses;

public class CustomerCardsResponse
{
    public required IEnumerable<CustomerCardResponse> Items { get; init; } = Enumerable.Empty<CustomerCardResponse>();
}