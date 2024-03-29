using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Components.Dialogs;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Pages;

public partial class CategoryTable
{
    private int _sortType = 0;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Category> _items = Enumerable.Empty<Category>().AsQueryable();
    protected override void OnInitialized()
    {
        _items = GenerateSampleGridData(5000);
    }
    
    private async Task OpenCreateDialogAsync()
    {
        var context = new Category(0, "");
        var dialog = await DialogService.ShowDialogAsync<CreateEditCategoryDialog>(context, new DialogParameters
        {
            Height = "230px",
            Title = "Add new category",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            // DB
        }
    }

    private async Task OpenEditDialogAsync(Category context)
    {
        var dialog = await DialogService.ShowDialogAsync<CreateEditCategoryDialog>(context, new DialogParameters()
        {
            Height = "230px",
            Title = $"Updating category {context.Name}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            // DB
        }  
    }
    
    private IQueryable<Category> GenerateSampleGridData(int size)
    {
        Category[] data = new Category[size];

        for (int i = 0; i < size; i++)
        {
            data[i] = new Category(i, $"Category {i}");
        }
        return data.AsQueryable();
    }
}