namespace Zlagoda.Contracts.QueryParameters;

public class StoreProductQueryParameters
{
    public bool? Promo { get; set; }
    public IEnumerable<Guid>? Category { get; set; }
    public string? SortBy { get; set; } // name, quantity, etc.
    public string? SortOrder { get; set; } // asc, desc

    public string? StartUpc { get; set; }
    public string? SearchQuery { get; set; }
    public int? MinProductPerCheckCount { get; set; } = 0;
}