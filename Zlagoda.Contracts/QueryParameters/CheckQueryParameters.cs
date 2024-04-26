namespace Zlagoda.Contracts.QueryParameters;

public class CheckQueryParameters
{
    public Guid? Employee { get; set; }
    public DateTime? PrintTimeStart { get; set; }
    public DateTime? PrintTimeEnd { get; set; }
    public string? StartIdCheck { get; set; }
    public bool WithProductsFromAllCategories { get; set; }
}