namespace SupermarketZlagoda.Data.Model;

public record CustomerCard(
    string IdCardCustomer,
    string CustomerSurname,
    string CustomerName,
    string CustomerPatronymic,
    string CustomerPhone,
    string CustomerCity,
    string CustomerStreet,
    string CustomerIndex,
    int CustomerPercentage)
    {
    public string CustomerSurname { get; set; } = CustomerSurname;
    public string CustomerName { get; set; } = CustomerName;
    public string CustomerPatronymic { get; set; } = CustomerPatronymic;
    public string CustomerPhone { get; set; } = CustomerPhone;
    public string CustomerCity { get; set; } = CustomerCity;
    public string CustomerStreet { get; set; } = CustomerStreet;
    public string CustomerIndex { get; set; } = CustomerIndex;
    public int CustomerPercentage { get; set; } = CustomerPercentage;
    
}