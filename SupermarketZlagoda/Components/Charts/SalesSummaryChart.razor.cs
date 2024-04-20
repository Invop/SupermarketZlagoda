using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Charts;

public partial class SalesSummaryChart : ComponentBase
{
    private string? _selectedEmployeeId;

    private string? SelectedEmployeeId
    {
        get => _selectedEmployeeId;
        set
        {
            if (_selectedEmployeeId == value) return;
            _selectedEmployeeId = value;
            _ = UpdateChartData();
        }
    }

    private Employee? SelectedEmployee { get; set; }
    private string? _selectedProductId;

    private string? SelectedProductId
    {
        get => _selectedProductId;
        set
        {
            if (_selectedProductId == value) return;
            _selectedProductId = value;
            _ = UpdateChartData();
        }
    }

    private Product? SelectedProduct { get; set; }

    private DateTime? _dateTimeFrom;

    private DateTime? DateTimeFrom
    {
        get => _dateTimeFrom;
        set
        {
            if (_dateTimeFrom == value) return;
            _dateTimeFrom = value;
            _ = UpdateChartData();
        }
    }

    private DateTime? _dateTimeTo;

    private DateTime? DateTimeTo
    {
        get => _dateTimeTo;
        set
        {
            if (_dateTimeTo == value) return;
            _dateTimeTo = value;
            _ = UpdateChartData();
        }
    }


    private readonly List<Product> _products = [new Product { Id = Guid.Empty, Name = "None" }];
    private readonly List<Employee> _employees = [new Employee { Id = Guid.Empty, Surname = "None" }];
    private List<SaleSummary> _sales = new();


    private bool _showDataLabels;

    private class DataItem
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    private static readonly HttpClient Client = new();
    private DataItem[] _chartData = [];
    private decimal SaleAmount { get; set; }


    protected override async Task OnInitializedAsync()
    {
        _employees.AddRange(await GetAllEmployeesAsync() ?? []);
        _products.AddRange(await GetAllStoreProductsAsync() ?? []);
        _sales = await GetAllSalesSummary() ?? [];
        _chartData = _sales.Select(s =>
                new DataItem { Name = s.ProductName ?? string.Empty, Price = s.SellingPrice ?? 0 })
            .ToArray();
        SaleAmount = _chartData.Sum(x => x.Price);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task UpdateChartData()
    {
        _sales = await GetAllSalesSummary() ?? [];
        _chartData = _sales.Select(s =>
                new DataItem { Name = s.ProductName ?? string.Empty, Price = s.SellingPrice ?? 0 })
            .ToArray();
        SaleAmount = _chartData.Sum(x => x.Price);
        StateHasChanged();
    }

    #region ApiCalls

    private async Task<List<Product>?> GetAllStoreProductsAsync()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/products");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var productList =
                JsonConvert.DeserializeObject<List<Product>>(JObject.Parse(responseJson)["items"]?.ToString() ??
                                                             string.Empty);
            if (!productList.IsNullOrEmpty()) return productList;
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        return null;
    }

    private async Task<List<Employee>?> GetAllEmployeesAsync()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/employees");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var employeeList =
                JsonConvert.DeserializeObject<List<Employee>>(JObject.Parse(responseJson)["items"]?.ToString() ??
                                                              string.Empty);
            if (!employeeList.IsNullOrEmpty()) return employeeList;
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        return null;
    }

    private async Task<List<SaleSummary>?> GetAllSalesSummary()
    {
        var employeeId = SelectedEmployee?.Id != Guid.Empty ? SelectedEmployeeId : null;
        var productId = SelectedProduct?.Id != Guid.Empty ? SelectedProductId : null;
        var formattedFromDate = DateTimeFrom?.ToString("yyyy-MM-dd HH:mm:ss");
        var formattedToDate = DateTimeTo?.ToString("yyyy-MM-dd HH:mm:ss");
        var response =
            await Client.GetAsync(
                $"https://localhost:5001/api/sale/summary?EmployId={employeeId}&ProductId={productId}&PeriodFrom={formattedFromDate}&PeriodTo={formattedToDate}");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var salesList =
                JsonConvert.DeserializeObject<List<SaleSummary>>(JObject.Parse(responseJson)["items"]?.ToString() ??
                                                                 string.Empty);
            if (!salesList.IsNullOrEmpty()) return salesList;
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        return null;
    }

    #endregion

    private void ResetDateClick()
    {
        _dateTimeFrom = null;
        _dateTimeTo = null;
    }
}