namespace Zlagoda.Contracts.QueryParameters;

public class ProductQueryParameters
{
    public IEnumerable<Guid>? Category { get; set; }
    public string? SortBy { get; set; } // name
    public string? SortOrder { get; set; }// asc, desc
    
    public string? ProductTitleMatch { get; set; }//the product must have this substring in its name
    
}