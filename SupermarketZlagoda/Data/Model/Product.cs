﻿namespace SupermarketZlagoda.Data.Model;

public record Product
{
    public Guid Id { get; init; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Characteristics { get; set; }
}