namespace SupermarketZlagoda.Data.Model;

public record CustomerCard()
    {
        
    public Guid Id { get; init; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public string Phone { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Index { get; set; }
    public int Percentage { get; set; }
    
}