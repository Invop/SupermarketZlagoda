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
    private string _surnameSearchTerm = string.Empty;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Employee> _items = Enumerable.Empty<Employee>().AsQueryable();
    private static readonly HttpClient Client = new HttpClient();
    
    protected override async Task OnInitializedAsync()
    {
        await UpdateTable();
    }
    private async Task OpenCreateDialogAsync()
    {
        var context = new Employee()
        {
            EmployeeSurname = "",
            EmployeeName = "",
            Role = "",
            Salary = 0,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today),
            DateOfStart = DateOnly.FromDateTime(DateTime.Today),
            PhoneNumber = "",
            City = "",
            Street = "",
            ZipCode = ""
        };
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
            var item = result.Data as Employee;
            await PostEmployeeAsync(item);
            await UpdateTable();
        }
    }
    
    private async Task UpdateTable()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/employees");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var employeeList = JsonConvert.DeserializeObject<List<Employee>>(JObject.Parse(responseJson)["items"].ToString());
            _items = employeeList.AsQueryable();
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
            var item = result.Data as Employee;
            await UpdateEmployeeAsync(item);
            await UpdateTable();
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
            = await client.PutAsync($"https://localhost:5001/api/employees/{employee.IdEmployee}", content);

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
{context.EmployeeSurname} {context.EmployeeName} {context.EmployeePatronymic}?"),
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
            await DeleteEmployeeAsync(context.IdEmployee);
            await UpdateTable();
        }
    }
    
    private async Task DeleteEmployeeAsync(string id)
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