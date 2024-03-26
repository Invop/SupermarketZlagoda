using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Components.Dialogs;
using SupermarketZlagoda.Data.Model;
namespace SupermarketZlagoda.Components.Pages;

public partial class EmployeeTable
{
    private int _sortType = 0;
    private bool _cashiersOnly = false;
    private string _surnameSearchTerm = string.Empty;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Employee> _items = Enumerable.Empty<Employee>().AsQueryable();
    protected override void OnInitialized()
    {
        _items = GenerateSampleGridData(5000);
    }
    
    private async Task OpenCreateDialogAsync()
    {
        var context = new Employee(0, "", "",
            "", "", 0m, DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today),
            "", "", "", "");
        var dialog = await DialogService.ShowDialogAsync<CreateEditEmployeeDialog>(context, new DialogParameters()
        {
            Height = "1000px",
            Title = "Add new employee",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            // DB
        }
    }
    
    private async Task OpenEditDialogAsync(Employee context)
    {
        var dialog = await DialogService.ShowDialogAsync<CreateEditEmployeeDialog>(context, new DialogParameters()
        {
            Height = "1020px",
            Title = $"Updating {context.EmployeeSurname} {context.EmployeeName} {context.EmployeePatronymic}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            // DB
        }  
    }
    
    private IQueryable<Employee> GenerateSampleGridData(int size)
    {
        Employee[] data = new Employee[size];

        for (int i = 0; i < size; i++)
        {
            data[i] = new Employee(i, $"Surname {i}", $"Name {i}",
                $"Patronymic {i}", $"Role {i}", i * 100m,
                new DateOnly(2000, 1, 1), new DateOnly(2000, 1, 1),
                $"+380555{i}", $"City {i}", $"Street {i}", $"{i}");
        }
        return data.AsQueryable();
    }
}