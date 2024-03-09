namespace SupermarketZlagoda.Data.Model;

public record Check(string IdCheck, string IdEmployee, string IdCardCustomer, DateTime PrintDate,
    decimal SumTotal, decimal Vat);