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

    private static readonly HttpClient Client = new HttpClient();

    protected override async Task OnInitializedAsync()
    {
        IsManager = UserState.IsManager;
        await UpdateTable();
    }

    private async Task UpdateTable()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/customer-card");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var customerCardList =
                JsonConvert.DeserializeObject<List<CustomerCard>>(JObject.Parse(responseJson)["items"].ToString());
            _items = customerCardList.AsQueryable();
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
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
            await UpdateTable();
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
            Title = $"Updating the {context.Surname} {context.Name} {context.Patronymic}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as CustomerCard;
            await PostCustomerCardAsync(item);
            await UpdateTable();
        }
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
            await UpdateTable();
        }
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
}