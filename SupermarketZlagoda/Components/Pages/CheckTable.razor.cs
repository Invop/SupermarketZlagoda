using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Data;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Pages;
public partial class CheckTable
{
    private bool IsManager { get; set; } = false;
    private string CheckSearchTerm = string.Empty;
    private int _sortType = 0;

    private string? PeriodValue = "all";
    
    private decimal TotalSum;
    private string? employeeValue;
    Option<string>? selectedEmployeeOption = employeeOptions[0];
    
    private DateTime? DateFromValue;
    private DateTime? DateToValue;
    
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Check>? _items = Enumerable.Empty<Check>().AsQueryable();
    static List<Option<string>> employeeOptions = new()
    {
        { new Option<string> { Value = "All", Text = "All" } },
        { new Option<string> { Value = "First", Text = "First"} },
        { new Option<string> { Value = "Second", Text = "Second" } },
        { new Option<string> { Value = "Third", Text = "Third" } },
        
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

    private IQueryable<Check> GenerateSampleGridData(int size)
    {
        Check[] data = new Check[size];
        for (int i = 0; i < size; i++)
        {
            data[i] = new Check($"{i}", $"7{i}", $"8{i}", new DateTime(2024, 3, 6),
                123.45m, 0.45m);
            TotalSum += data[i].SumTotal;
        }
        return data.AsQueryable();
    }
    protected override void OnInitialized()
    {
        _items = GenerateSampleGridData(5000);
    }
}