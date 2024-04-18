namespace Zlagoda.Contracts.QueryParameters;

public class CustomerCardQueryParameters
{
     public int? Percentage { get; set; }
     public string? SortBy { get; set; } // name, quantity, etc.
     public string? SortOrder { get; set; }// asc,a desc
  
     public string? StartSurname { get; set; }

}