using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Pages;

public partial class ProductTable
{
    private bool IsManager { get; set; } = false;
    
    private string _searchTerm = string.Empty;
    private int _sortType = 0;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Product>? _items = Enumerable.Empty<Product>().AsQueryable();

    protected override Task OnInitializedAsync()
    {
        IsManager = UserState.IsManager;
        return base.OnInitializedAsync();
    }

    private IQueryable<Product> GenerateSampleGridData(int size)
    {
        Product[] data = new Product[size];

        for (int i = 0; i < size; i++)
        {
            data[i] = new Product(i, i, $"Product {i}", $"Char {i}");
        }
        return data.AsQueryable();
    }
    protected override void OnInitialized()
    {
        _items = GenerateSampleGridData(5000);
    }
}