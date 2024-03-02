namespace SupermarketZlagoda.Data.Model;

public record CustomerCard(string IdCardCustomer, string CustomerSurname, string CustomerName, string CustomerPatronymic, 
string CustomerPhone, string CustomerCity, string CustomerStreet, string CustomerIndex, int CustomerPercentage);