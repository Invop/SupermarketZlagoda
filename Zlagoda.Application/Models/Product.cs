﻿namespace Zlagoda.Application.Models;

public class Product
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required Guid CategoryId { get; set; }
    public required string Characteristics { get; set; }
}