﻿using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupermarketZlagoda.Components.Dialogs;
using SupermarketZlagoda.Data;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Pages;

public partial class CustomerCardTable
{
    private bool IsManager { get; set; } = false;
    private int _sortType = 0;
    private string _searchTerm = string.Empty;
    private string? percentageValue;
    private Option<int?> selectedPercentageOption;

    private DateTime? _dateFromValue = null;
    private DateTime? _dateToValue = null;

    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<CustomerCard>? _items = Enumerable.Empty<CustomerCard>().AsQueryable();
    static List<Option<int?>> PercentageOptions = new() { };
    private static readonly HttpClient Client = new HttpClient();

    private DateTime? DateFromValue
    {
        get => _dateFromValue;
        set
        {
            _dateFromValue = value;
            _ = GetCustomerCardAsync();
        }
    }

    private DateTime? DateToValue
    {
        get => _dateToValue;
        set
        {
            _dateToValue = value;
            _ = GetCustomerCardAsync();
        }
    }

    private int SortType
    {
        get => _sortType;
        set
        {
            _sortType = value;
            _ = GetCustomerCardAsync();
        }
    }

    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            _searchTerm = value;
            _ = GetCustomerCardAsync();
        }
    }

    private IQueryable<CustomerCard>? _zapitdata = Enumerable.Empty<CustomerCard>().AsQueryable();

    private async Task OnSelectedPercentageChanged()
    {
        await GetCustomerCardAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        IsManager = User.IsManager;
        await GetZapitData();
        await GetCustomerCardAsync();
        await GetPercentageAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var userRole = await localStorage.ContainKeyAsync("IsManager");
            if (userRole)
            {
                User.IsManager = await localStorage.GetItemAsync<bool>("IsManager");
                StateHasChanged();
            }
        }
    }

    #region Api

// Приватний асинхронний метод отримання даних запиту
    private async Task GetZapitData()
    {
        // Отримання відповіді сервера на наш запит
        var response = await Client.GetAsync("https://localhost:5001/api/customer-card/zapit");
        // Якщо стан відповіді успішний
        if (response.IsSuccessStatusCode)
        {
            // Читання вмісту рядка у форматі JSON
            var responseJson = await response.Content.ReadAsStringAsync();
            // Десереалізація рядка JSON і перетворення на список об'єктів типу CustomerCard
            _zapitdata = JsonConvert
                .DeserializeObject<List<CustomerCard>>(JObject.Parse(responseJson)["items"].ToString())
                .AsQueryable();
        }
    }

    private async Task GetPercentageAsync()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/customer-card");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var customerCardList =
                JsonConvert.DeserializeObject<List<CustomerCard>>(JObject.Parse(responseJson)["items"].ToString());

            var uniquePercentages = customerCardList.Select(card => card.Percentage).Distinct().OrderBy(value => value)
                .ToList();

            PercentageOptions = uniquePercentages.Select(value => new Option<int?> { Value = value, Text = value })
                .ToList();
            PercentageOptions.Insert(0, new Option<int?> { Value = null, Text = null });
        }
    }

    private async Task GetCustomerCardAsync()
    {
        var formattedFromDate = _dateFromValue?.ToString("yyyy-MM-dd HH:mm:ss");
        var formattedToDate = _dateToValue?.ToString("yyyy-MM-dd HH:mm:ss");
        var sortType = _sortType == 0 ? "asc" : "desc";
        var response =
            await Client.GetAsync(
                $"https://localhost:5001/api/customer-card/?SortBy=cust_surname,cust_name,cust_patronymic&SortOrder={sortType}&StartSurname={_searchTerm}&Percentage={selectedPercentageOption?.Value}&StartDate={formattedFromDate}&EndDate={formattedToDate}");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var customerCardList =
                JsonConvert.DeserializeObject<List<CustomerCard>>(JObject.Parse(responseJson)["items"].ToString());
            if (customerCardList != null) _items = customerCardList.AsQueryable();
            StateHasChanged();
            await GetPercentageAsync();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }

    private async Task UpdateCustomerCardAsync(CustomerCard customerCard)
    {
        var customerCardJson = JsonConvert.SerializeObject(customerCard);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(customerCardJson, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"https://localhost:5001/api/customer-card/{customerCard.Id}",
            content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Customer card successfully updated."
            : $"Failed to update the customer card. Status code: {response.StatusCode}");
    }


    private async Task PostCustomerCardAsync(CustomerCard customerCard)
    {
        var customerCardJson = JsonConvert.SerializeObject(customerCard);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(customerCardJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:5001/api/customer-card", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Customer card successfully saved."
            : $"Failed to save the customer card. Status code: {response.StatusCode}");
    }

    private async Task DeleteCustomerCardAsync(Guid guid)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.DeleteAsync($"https://localhost:5001/api/customer-card/{guid}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Customer card successfully deleted.");
        }
        else
        {
            Console.WriteLine($"Failed to delete the customer card. Status code: {response.StatusCode}");
        }
    }

    #endregion

    #region Dialogs

    private async Task OpenCreateDialogAsync()
    {
        var context = new CustomerCard()
        {
            Surname = "",
            Name = "",
            Patronymic = "",
            Phone = "",
            City = "",
            Street = "",
            Index = "",
            Percentage = 0
        };
        var dialog = await DialogService.ShowDialogAsync<CreateEditCustomerCardDialog>(context, new DialogParameters()
        {
            Height = "850px",
            Title = $"Add new customer card",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as CustomerCard;
            await PostCustomerCardAsync(item);
            await GetCustomerCardAsync();
        }
    }

    private async Task OpenEditDialogAsync(CustomerCard context)
    {
        var dialog = await DialogService.ShowDialogAsync<CreateEditCustomerCardDialog>(context, new DialogParameters()
        {
            Height = "850px",
            Title = $"Updating the {context.Surname} {context.Name} {context.Patronymic}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as CustomerCard;
            await UpdateCustomerCardAsync(item);
            await GetCustomerCardAsync();
        }
    }

    private async Task OpenDeleteDialog(CustomerCard context)
    {
        var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
        {
            Content = new MessageBoxContent
            {
                Title = "Warning",
                MarkupMessage =
                    new MarkupString(
                        $"Are you sure you want to delete {context.Surname} {context.Name} {context.Patronymic}?"),
                Icon = new Icons.Regular.Size24.Warning(),
                IconColor = Color.Error,
            },
            PrimaryAction = "Yes",
            SecondaryAction = "No",
            Width = "300px",
        });
        var result = await dialog.Result;
        var canceled = result.Cancelled;
        if (!canceled)
        {
            await DeleteCustomerCardAsync(context.Id);
            await GetCustomerCardAsync();
        }
    }

    #endregion

    private async Task SetTodayDate()
    {
        _dateFromValue = DateTime.Now.Date;
        _dateToValue = DateTime.Now.Date.AddDays(1).AddTicks(-1);
        await GetCustomerCardAsync();
    }

    private async Task ResetDate()
    {
        _dateFromValue = null;
        _dateToValue = null;
        await GetCustomerCardAsync();
    }

    private async Task PrintTable()
    {
        var printer = new TablePrinter<CustomerCard>(_items);
        var tableContent = printer.PrintTable();
        await IJS.InvokeVoidAsync("printComponent", tableContent, "Customer cards");
    }

    private async Task ShowInformationAsync()
    {
        var dialog =
            await DialogService.ShowInfoAsync(
                "Для кожної карти клієнта вивести кількість одиниць товарів, куплених за вказаний період");
        var result = await dialog.Result;
    }
}