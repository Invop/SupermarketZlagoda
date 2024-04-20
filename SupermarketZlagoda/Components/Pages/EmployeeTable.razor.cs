using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupermarketZlagoda.Components.Dialogs;
using SupermarketZlagoda.Data.Model;
namespace SupermarketZlagoda.Components.Pages;

public partial class EmployeeTable
{
    private int _sortType = 0;
    private bool _cashiersOnly = false;
    private string _searchTerm = string.Empty;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Employee> _items = Enumerable.Empty<Employee>().AsQueryable();
    private static readonly HttpClient Client = new();
    
    protected override async Task OnInitializedAsync()
    {
        await GetEmployeesAsync();
    }
    
    private int SortType
    {
        get => _sortType;
        set { _sortType = value;
            _ = GetEmployeesAsync();
        }
    }
    
    public string SearchTerm
    {
        get => _searchTerm;
        set { _searchTerm = value;  _ = GetEmployeesAsync();}
    }
    
    public bool CashiersOnly
    {
        get => _cashiersOnly;
        set { _cashiersOnly = value;  _ = GetEmployeesAsync();}
    }
    
    private async Task GetEmployeesAsync()
    {
        var sortType = _sortType == 0 ? "asc" : "desc";
        var response = await Client.GetAsync($"https://localhost:5001/api/employees/?SortBy=empl_surname {sortType}, empl_name {sortType}, empl_patronymic {sortType}&CashiersOnly={_cashiersOnly}&StartSurname={_searchTerm}");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<List<Employee>>(JObject.Parse(responseJson)["items"].ToString());
            if (employees != null) _items = employees.AsQueryable();
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
    
    private async Task PostEmployeeAsync(Employee employee)
    {
        var employeeJson = JsonConvert.SerializeObject(employee);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:5001/api/employees", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Employee successfully saved."
            : $"Failed to save the employee. Status code: {response.StatusCode}");
    }
    
    private async Task OpenCreateDialogAsync()
    {
        var context = new Employee
        {
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today),
            DateOfStart = DateOnly.FromDateTime(DateTime.Today)
        };
        var dialog = await DialogService.ShowDialogAsync<CreateEditEmployeeDialog>(context, new DialogParameters()
        {
            Title = "Add new employee",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as Employee;
            await PostEmployeeAsync(item);
            await GetEmployeesAsync();
        }
    }
    
    private async Task OpenEditDialogAsync(Employee context)
    {
        var dialog = await DialogService.ShowDialogAsync<CreateEditEmployeeDialog>(context, new DialogParameters()
        {
            Title = $"Updating {context.Surname} {context.Name} {context.Patronymic}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as Employee;
            await UpdateEmployeeAsync(item);
            await GetEmployeesAsync();
        }  
    }
    
    private async Task UpdateEmployeeAsync(Employee employee)
    {
        var productJson = JsonConvert.SerializeObject(employee);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(productJson, Encoding.UTF8, "application/json");

        var response
            = await client.PutAsync($"https://localhost:5001/api/employees/{employee.Id}", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Employee successfully updated."
            : $"Failed to update the employee. Status code: {response.StatusCode}");
    }
    
    private async Task OpenDeleteDialogAsync(Employee context)
    {
        var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
        {
            Content = new MessageBoxContent
            {
                Title = "Warning",
                MarkupMessage = new MarkupString(@$"Are you sure you want to delete
{context.Surname} {context.Name} {context.Patronymic}?"),
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
            await DeleteEmployeeAsync(context.Id);
            await GetEmployeesAsync();
        }
    }
    
    private async Task DeleteEmployeeAsync(Guid id)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.DeleteAsync($"https://localhost:5001/api/employees/{id}");

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Employee successfully deleted."
            : $"Failed to delete the employee. Status code: {response.StatusCode}");
    }
}