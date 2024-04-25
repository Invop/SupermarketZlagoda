namespace Zlagoda.Contracts.QueryParameters;

public class CategoryQueryParameters
{
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }// asc, desc
    public int? MinStoreProdCount { get; set; }
}