namespace Zlagoda.Contracts.Responses;

public class CustomerCardResponse {
    public required Guid Id { get; init; }
    public required string Surname { get; set; }
    public required string Name { get; set; }
    public string? Patronymic { get; set; }
    public required string Phone { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Index { get; set; }
    public required int Percentage { get; set; }
    
    public required int ProductNumber { get; init; }
}