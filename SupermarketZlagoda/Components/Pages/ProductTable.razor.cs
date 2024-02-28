using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Data;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Pages;

public partial class ProductTable
{
    private bool IsManager { get; set; } = false;
    
    private string _searchTerm = string.Empty;
    private int _sortType = 0;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Product>? _items = Enumerable.Empty<Product>().AsQueryable();
    private List<SelectOption> CategoryOptions = new List<SelectOption>()
    {
        new() { Value = "1", Text = "Technology", Selected = true },
        new() { Value = "2", Text = "Science", Selected = true },
        new() { Value = "3", Text = "Art & Culture" },
        new() { Value = "4", Text = "Health & Wellness" },
        new() { Value = "5", Text = "Sports" },
        new() { Value = "6", Text = "Education" },
        // ... other category options
    };

    protected override async Task OnInitializedAsync()
    {
        IsManager = UserState.IsManager;
        var items = await SqliteDataAccess.FetchProductsData();
        _items = items.AsQueryable();
    }
    
    private void HandleSelectCategoryChange(List<SelectOption> selectedOptions)
    {
        foreach (var selectedOption in selectedOptions.Where(selectedOption => selectedOption.Selected))
        {
            Console.WriteLine(selectedOption.Value);
        }
    }
    
}