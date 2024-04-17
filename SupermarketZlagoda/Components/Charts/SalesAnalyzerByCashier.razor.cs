using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupermarketZlagoda.Data;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Charts;

public partial class SalesAnalyzerByCashier : ComponentBase
{
    private DateTime? _dateFrom =DateTime.Today;
    private DateTime? _dateTo = DateTime.Today + TimeSpan.FromDays(1);
    private PieChart pieChart = default!;
    private PieChartOptions pieChartOptions = default!;
    private ChartData chartData = default!;
    private string[]? backgroundColors;

    private DateTime? DateFrom
    {
        get => _dateFrom;
        set
        {
            _dateFrom = value;
            if (_dateFrom is null) return;
            UpdateDataAndCharts();
        }
    }

    private DateTime? DateTo
    {
        get => _dateTo;
        set
        {
            _dateTo = value;
            if (_dateTo is null) return;
            UpdateDataAndCharts();
        }
    }
    public Employee? SelectedEmployee { get; set; }
    
    private int datasetsCount = 0;
    private int dataLabelsCount = 0;
    public string? SelectedEmployeeValue { get; set; }

    private static readonly HttpClient Client = new HttpClient();
    private List<Employee>? _employees;
    private List<SoldProduct>? _products;

    private async Task GetAllEmployees()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/employees");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            _employees = JsonConvert.DeserializeObject<List<Employee>>(JObject.Parse(responseJson)["items"].ToString());
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
    private async Task GetAllSoldProducts()
    {
        var startDate = DateFrom?.ToString("yyyy-MM-dd");
        var endDate = DateTo?.ToString("yyyy-MM-dd");

        var response = await Client.GetAsync($"https://localhost:5001/api/sold-products?CashierId={SelectedEmployee?.Id}&StartDate={startDate}&EndDate={endDate}");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            _products = 
                JsonConvert.DeserializeObject<List<SoldProduct>>(JObject.Parse(responseJson)["items"].ToString());
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }

    private async Task SetUpData()
    {
        _products?.Clear();
        await GetAllSoldProducts();
    }

    private void UpdateCharts()
    {
        backgroundColors = ColorBuilder.CategoricalTwelveColors;
        chartData = new ChartData { Labels = GetDefaultDataLabels(), Datasets = GetDefaultDataSets(1) };

        pieChartOptions = new PieChartOptions();
        pieChartOptions.Responsive = true;
        pieChartOptions.Plugins.Title!.Text = "2022 - Sales";
        pieChartOptions.Plugins.Title.Display = true;
    }
    private async void UpdateDataAndCharts()
    {
        await SetUpData();
        UpdateCharts();
        _ = pieChart.UpdateAsync(chartData, pieChartOptions);
    }

    protected override async Task OnInitializedAsync()
    {   
        
        await GetAllEmployees();
        if (_employees != null) SelectedEmployeeValue = _employees.FirstOrDefault()?.Id.ToString();
        await SetUpData();

        backgroundColors = ColorBuilder.CategoricalTwelveColors;
        chartData = new ChartData { Labels = GetDefaultDataLabels(), Datasets = GetDefaultDataSets(1) };

        pieChartOptions = new PieChartOptions();
        pieChartOptions.Responsive = true;
        pieChartOptions.Plugins.Title!.Text = "2022 - Sales";
        pieChartOptions.Plugins.Title.Display = true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await pieChart.InitializeAsync(chartData, pieChartOptions);
        await base.OnAfterRenderAsync(firstRender);
    }
    

    #region Data Preparation

    private List<IChartDataset> GetDefaultDataSets(int numberOfDatasets)
    {
        var datasets = new List<IChartDataset>();

        for (var index = 0; index < numberOfDatasets; index++) datasets.Add(GetRandomPieChartDataset());

        return datasets;
    }

    private PieChartDataset GetRandomPieChartDataset()
    {
        datasetsCount += 1;
        if (_products != null && _products.Count > datasetsCount)
        {
            return new PieChartDataset
                { Label = $"Ціна : {_products[datasetsCount].SellingPrice} Кількість :", Data = GetRandomData(), BackgroundColor = GetRandomBackgroundColors() };
        }

        return new PieChartDataset();
    }

    private List<double> GetRandomData()
    {
        var data = new List<double>();
        for (var index = 0; index < dataLabelsCount; index++)
        {
            if (_products != null && index < _products.Count)
                data.Add(_products[index].TotalQuantity);
        }

        return data;
    }

    private List<string> GetRandomBackgroundColors()
    {
        var colors = new List<string>();
        for (var index = 0; index < dataLabelsCount; index++) colors.Add(backgroundColors![index]);

        return colors;
    }

    private List<string> GetDefaultDataLabels()
    {
        var labels = new List<string>();
        if (_products == null) return labels;
        foreach (var t in _products)
        {
            labels.Add(t.ProductName);
            dataLabelsCount += 1;
        }

        return labels;
    }

    #endregion Data Preparation

}