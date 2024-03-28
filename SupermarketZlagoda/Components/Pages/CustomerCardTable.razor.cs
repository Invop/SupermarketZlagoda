using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Data;
using SupermarketZlagoda.Data.Model;
using SupermarketZlagoda.Components.Dialogs;

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
        for (int i = 0; i < size; i++)
        {
            data[i] = new CustomerCard($"{i}", $"surnameeeeeeeeeeeeeee{i}", $"name{i}", $"patronymic{i}",
                "+380000000000", $"city nameeeeeeeeeeeeeeeee{i}", $"street {i}",$"396{i}", 10);
        }
        return data.AsQueryable();
    }
    protected override void OnInitialized()
    {
        _items = GenerateSampleGridData(5000);
    }
    private async Task OpenEditDialogAsync(CustomerCard context)
    {
        var dialog = await DialogService.ShowDialogAsync<CreateEditCustomerCardDialog>(context, new DialogParameters()
        {
            Height = "850px",
            Title = $"Updating the {context.CustomerSurname} {context.CustomerName} {context.CustomerPatronymic}",
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
        var context = new CustomerCard(string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, string.Empty, string.Empty, string.Empty, 0);
        var dialog = await DialogService.ShowDialogAsync<CreateEditCustomerCardDialog>(context, new DialogParameters()
        {
            Height = "850px",
            Title = $"Updating the {context.CustomerSurname} {context.CustomerName} {context.CustomerPatronymic}",
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