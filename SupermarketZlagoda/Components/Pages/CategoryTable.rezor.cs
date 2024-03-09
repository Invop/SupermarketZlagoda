using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Pages;

public partial class CategoryTable
{
    private int _sortType = 0;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Category>? _items = Enumerable.Empty<Category>().AsQueryable();

    private IQueryable<Category> GenerateSampleGridData(int size)
    {
        Category[] data = new Category[size];

        for (int i = 0; i < size; i++)
        {
            data[i] = new Category(i, $"Category {i}");
        }
        return data.AsQueryable();
    }
    protected override void OnInitialized()
    {
        _items = GenerateSampleGridData(5000);
    }
}