using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Components.Dialogs;
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
    protected override Task OnInitializedAsync()
    {
        IsManager = UserState.IsManager;
        return base.OnInitializedAsync();
    }
    
    private void HandleSelectCategoryChange(List<SelectOption> selectedOptions)
    {
        foreach (var selectedOption in selectedOptions.Where(selectedOption => selectedOption.Selected))
        {
            Console.WriteLine(selectedOption.Value);
        }
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

    private async Task OpenEditDialogAsync(Product context)
    {
            var dialog = await DialogService.ShowDialogAsync<CreateEditProductDialog>(context, new DialogParameters()
            {
                Height = "400px",
                Title = $"Updating the {context.ProductName}",
                PreventDismissOnOverlayClick = true,
                PreventScroll = true,
            });

            var result = await dialog.Result;
            if (result is { Cancelled: false, Data: not null })
            {
                //Update DB
            }
            
    }
    private async Task OpenCreateDialogAsync()
    {   
        var context = new Product(0, 0, string.Empty, string.Empty);
        var dialog = await DialogService.ShowDialogAsync<CreateEditProductDialog>(context, new DialogParameters()
        {
            Height = "400px",
            Title = $"Updating the {context.ProductName}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            //Update DB
        }
            
    }
}