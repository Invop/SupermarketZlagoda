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
public partial class CheckTable
{
    private bool IsManager { get; set; } = false;
    private string CheckSearchTerm = string.Empty;
    private int _sortType = 0;
    
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

    private static readonly HttpClient Client = new HttpClient();
    private Dictionary<Guid, string> _employees = new();
    private List<SelectOption> _employeesOptions = [];
    
    private Dictionary<Guid, string>? _customerCards = new();
    private List<SelectOption> _customerCardsOptions = [];
    
    private Dictionary<Guid, string>? _storeProducts = new();
    private List<SelectOption> _storeProductsOptions = [];
    
    public static List<Sale> SalesList { get; set; } = new List<Sale>();
    protected override async Task OnInitializedAsync()
    {
        IsManager = UserState.IsManager;
        await UpdateEmployeeOptions();
        await UpdateCustomerOptions();
        await UpdateStoreProductOptions();
        await UpdateTable();
    }
    private async Task UpdateEmployeeOptions()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/employees");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var employees = JsonConvert
                .DeserializeObject<List<Employee>>(JObject.Parse(responseJson)["items"].ToString());
            foreach (var employee in employees)
            {
                _employees[employee.Id] = employee.Surname;
                _employeesOptions.Add(new SelectOption(employee.Id, employee.Surname));
            }
        }
        else
        {
            Console.WriteLine($"Error Employee: {response.StatusCode}");
        }
    }
    
    private async Task UpdateCustomerOptions()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/customer-card");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert
                .DeserializeObject<List<CustomerCard>>(JObject.Parse(responseJson)["items"].ToString());
            
            foreach (var customerCard in customers)
            {
                _customerCards[customerCard.Id] = customerCard.Surname;
                _customerCardsOptions.Add(new SelectOption(customerCard.Id, customerCard.Surname));
            }
        }
        else
        {
            Console.WriteLine($"Error Customer: {response.StatusCode}");
        }
    }
    private async Task UpdateTable()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/check");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var checkList =
                JsonConvert.DeserializeObject<List<Check>>(JObject.Parse(responseJson)["items"].ToString());
            _items = checkList.AsQueryable();
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error Update: {response.StatusCode}");
        }
    }
    
    private async Task OpenCreateDialogAsync()
    {
        var context = new Check
        {
            IdEmployee = Guid.Empty,
            IdCardCustomer = Guid.Empty,
            PrintDate = DateTime.Now,
            SumTotal = 0,
            Vat = 0
        };
        var dialog = await DialogService.ShowDialogAsync<CreateCheckDialog>(context, new DialogParameters
        {
            Height = "auto",
            Width = "600px",
            Title = "Add new сheck",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });
        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as Check;
            await PostCheckAsync(item);
            await UpdateTable();
        }
    }
    
    private async Task PostSaleAsync(Sale sale)
    {
        var saleJson = JsonConvert.SerializeObject(sale);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(saleJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:5001/api/sale", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Sale successfully saved."
            : $"Failed to save the sale. Status code: {response.StatusCode}");
        
    }
    
    private async Task UpdateStoreProductOptions()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/store-products");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert
                .DeserializeObject<List<StoreProduct>>(JObject.Parse(responseJson)["items"].ToString());
            
            foreach (var storeProduct in customers)
            {
                _storeProducts[storeProduct.ProductId] = storeProduct.Upc;
                _storeProductsOptions.Add(new SelectOption(storeProduct.ProductId, storeProduct.Upc));
            }
        }
        else
        {
            Console.WriteLine($"Error Sale: {response.StatusCode}");
        }
    }
    
    private async Task PostCheckAsync(Check check)
    {
        var employeeJson = JsonConvert.SerializeObject(check);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:5001/api/check", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Check successfully saved."
            : $"Failed to save the check. Status code: {response.StatusCode}");

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var check1 = JsonConvert.DeserializeObject<Check>(responseContent);
            foreach (var sale in SalesList)
            {
                sale.CheckNumber = check1.IdCheck;
                await PostSaleAsync(sale);
            }
        }
        else
        {
            Console.WriteLine($"Failed to save the check. Status code: {response.StatusCode}");
        }
        
    }
    
    private async Task OpenDeleteDialogAsync(Check check)
    {
        var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
        {
            Content = new MessageBoxContent
            {
                Title = "Warning",
                MarkupMessage = new MarkupString($"Are you sure you want to delete {check.IdCheck}?"),
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
            await DeleteSaleAsync(check.IdCheck);
            await DeleteCheckAsync(check.IdCheck);
            await UpdateTable();
        }
    }
    private async Task DeleteSaleAsync(Guid guid)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.DeleteAsync($"https://localhost:5001/api/sale/{guid}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Sale successfully deleted.");
        }
        else
        {
            Console.WriteLine($"Failed to delete the check. Status code: {response.StatusCode}");
        }
    }
    private async Task DeleteCheckAsync(Guid guid)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.DeleteAsync($"https://localhost:5001/api/check/{guid}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Check successfully deleted.");
        }
        else
        {
            Console.WriteLine($"Failed to delete the check. Status code: {response.StatusCode}");
        }
    }
    
    private async Task OpenCheckAsync(Check check)
    {
        var dialog = await DialogService.ShowDialogAsync<OpenCheckDialog>(check, new DialogParameters()
        {
            Height = "auto",
            Title = "Details of the check",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });
        var result = await dialog.Result;
    }
}