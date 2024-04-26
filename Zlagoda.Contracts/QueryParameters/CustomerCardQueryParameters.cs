namespace Zlagoda.Contracts.QueryParameters;

public class CustomerCardQueryParameters
{
     public int? Percentage { get; set; }
     public string? SortBy { get; set; } // name, quantity, etc.
     public string? SortOrder { get; set; }// asc,a desc
  
     public string? StartSurname { get; set; }
     public DateTime? StartDate { get; set; } = new DateTime(1753, 01,01,12,12,12);
     public DateTime? EndDate { get; set; } = DateTime.Now.AddDays(1);

}