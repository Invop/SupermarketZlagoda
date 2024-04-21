using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
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
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<CustomerCard>? _items = Enumerable.Empty<CustomerCard>().AsQueryable();
    static List<Option<int?>> PercentageOptions = new() { };
    private static readonly HttpClient Client = new HttpClient();
    
    private int SortType
    {
        get => _sortType;
        set { _sortType = value;
            _ = GetCustomerCardAsync();
        }
    }
    
    public string SearchTerm
    {
        get => _searchTerm;
        set { _searchTerm = value;  _ = GetCustomerCardAsync();}
    }
    
    private async Task OnSelectedPercentageChanged()
    {
        await GetCustomerCardAsync();
    }
    
    protected override async Task OnInitializedAsync()
    {
        IsManager = User.IsManager;
        await GetCustomerCardAsync();
        await GetPercentageAsync();
    }
    
    #region Api
    private async Task GetPercentageAsync()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/customer-card");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var customerCardList = JsonConvert.DeserializeObject<List<CustomerCard>>(JObject.Parse(responseJson)["items"].ToString());
            
            var uniquePercentages = customerCardList.Select(card => card.Percentage).Distinct().OrderBy(value => value).ToList();
            
            PercentageOptions = uniquePercentages.Select(value => new Option<int?> { Value = value, Text = value}).ToList();
            PercentageOptions.Insert(0, new Option<int?> { Value = null, Text = null });
        }
    }
    
    private async Task GetCustomerCardAsync()
    {
        var sortType = _sortType == 0 ? "asc" : "desc";
        var response = await Client.GetAsync($"https://localhost:5001/api/customer-card/?SortBy=cust_surname,cust_name,cust_patronymic&SortOrder={sortType}&StartSurname={_searchTerm}&Percentage={selectedPercentageOption?.Value}");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var customerCardList =
                JsonConvert.DeserializeObject<List<CustomerCard>>(JObject.Parse(responseJson)["items"].ToString());
            if(customerCardList != null) _items = customerCardList.AsQueryable();
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
}