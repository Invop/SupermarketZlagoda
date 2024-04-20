using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Charts;

public partial class SalesSummary : ComponentBase
{
    private string? SelectedEmployeeId { get; set; }
    private Employee? SelectedEmployee { get; set; }

    private string? SelectedProductId { get; set; }
    private Product? SelectedStoreProduct { get; set; }

    private DateTime? DateTimeFrom { get; set; }
    private DateTime? DateTimeTo { get; set; }


    private readonly List<Product> _products = [new Product { Id = Guid.Empty, Name = "None" }];
    private readonly List<Employee> _employees = [new Employee { Id = Guid.Empty, Surname = "None" }];
    private BarChart barChart = default!;
    private BarChartOptions barChartOptions = default!;
    private ChartData chartData = default!;

    private int datasetsCount;
    private int labelsCount;

    private readonly string[] months =
    {
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November",
        "December"
    };

    private readonly Random random = new();

    private static readonly HttpClient Client = new();

    protected override async Task OnInitializedAsync()
    {
        _employees.AddRange(await GetAllEmployeesAsync() ?? []);
        _products.AddRange(await GetAllStoreProductsAsync() ?? []);
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await barChart.InitializeAsync(chartData, barChartOptions);

        await base.OnAfterRenderAsync(firstRender);
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

    #endregion


    #region Data Preparation

    private List<IChartDataset> GetDefaultDataSets(int numberOfDatasets)
    {
        var datasets = new List<IChartDataset>();

        for (var index = 0; index < numberOfDatasets; index++) datasets.Add(GetRandomBarChartDataset());

        return datasets;
    }

    private BarChartDataset GetRandomBarChartDataset()
    {
        var c = ColorBuilder.CategoricalTwelveColors[datasetsCount].ToColor();

        datasetsCount += 1;

        return new BarChartDataset
        {
            Label = $"Product {datasetsCount}",
            Data = GetRandomData(),
            BackgroundColor = new List<string> { c.ToRgbString() },
            BorderColor = new List<string> { c.ToRgbString() },
            BorderWidth = new List<double> { 0 }
        };
    }

    private List<double> GetRandomData()
    {
        var data = new List<double>();
        for (var index = 0; index < labelsCount; index++) data.Add(random.Next(200));

        return data;
    }

    private List<string> GetDefaultDataLabels()
    {
        var labels = new List<string>();
        for (var index = 0; index < months.Length; index++) labels.Add(GetNextDataLabel());

        return labels;
    }

    private string GetNextDataLabel()
    {
        labelsCount += 1;
        return months[labelsCount - 1];
    }

    #endregion Data Preparation
}