namespace Zlagoda.Contracts.Responses;

public class ChecksResponse
{
    public required IEnumerable<CheckResponse> Items { get; init; } = Enumerable.Empty<CheckResponse>();
}