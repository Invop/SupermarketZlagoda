using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Data;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Pages;
public partial class CustomerCardTable
{
    private bool IsManager { get; set; } = false;
    private string _surnameSearchTerm = string.Empty;
    private int _sortType = 0;
    
    private string? percentageValue;
    Option<int>? selectedPercentageOption;
    
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<CustomerCard>? _items = Enumerable.Empty<CustomerCard>().AsQueryable();
    static List<Option<int>> PercentageOptions = new()
    {
        { new Option<int> { Value = 10, Text = 10 } },
        { new Option<int> { Value = 20, Text = 20 } },
        { new Option<int> { Value = 30, Text = 30 } },
        { new Option<int> { Value = 90, Text = 90 } },
        
    };
    protected override Task OnInitializedAsync()
    {
        IsManager = UserState.IsManager;
        return base.OnInitializedAsync();
    }
    
    private void HandleSelectPercentageChange(List<SelectOption> selectedOptions)
    {
        foreach (var selectedOption in selectedOptions.Where(selectedOption => selectedOption.Selected))
        {
            Console.WriteLine(selectedOption.Value);
        }
    }

    private IQueryable<CustomerCard> GenerateSampleGridData(int size)
    {
        CustomerCard[] data = new CustomerCard[size];
        string p = ""; 
        for (int i = 0; i < size; i++)
        {
            // if (i % 2 == 0)
            // {
            //     p = $"patronymic{i}";
            // }
            // else
            // {
            //     p = "";
            // }
            data[i] = new CustomerCard($"{i}", $"surnameeeeeeeeeeeeeee{i}", $"name{i}", $"patronymic{i}",
                "+380000000000", $"city nameeeeeeeeeeeeeeeee{i}", $"street {i}",$"396{i}", 10);
        }
        return data.AsQueryable();
    }
    protected override void OnInitialized()
    {
        _items = GenerateSampleGridData(5000);
    }
}