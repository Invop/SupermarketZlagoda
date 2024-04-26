using System.Net.Http.Headers;
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

public partial class ProductTable
{
    private bool IsManager { get; set; } = false;

    private string _searchTerm = string.Empty;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Product>? _items = Enumerable.Empty<Product>().AsQueryable();
    private Dictionary<Guid, string> _categories = new();
    private List<SelectOption> _categoryOptions = [];
    private int _sortType = 0;
    private static readonly HttpClient Client = new HttpClient();

    private int SortType
    {
        get => _sortType;
        set
        {
            _sortType = value;
            _ = GetStoreProductsAsync();
        }
    }

    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            _searchTerm = value;
            _ = GetStoreProductsAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        IsManager = User.IsManager;
        await GetCategoryOptions();
        await GetStoreProductsAsync();
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

    private async Task GetCategoryOptions()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/categories");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert
                .DeserializeObject<List<Category>>(JObject.Parse(responseJson)["items"].ToString());
            foreach (var category in categories)
            {
                _categories[category.Id] = category.Name;
                _categoryOptions.Add(new SelectOption(category.Id, category.Name));
            }
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }

    private string GenerateQueryStringFromCategoryOptions()
    {
        return string.Join("&",
            _categoryOptions.Where(option => option.Selected).Select(option => $"Category={option.Value}"));
    }

    private async Task GetStoreProductsAsync()
    {
        var sortType = _sortType == 0 ? "asc" : "desc";
        var categoryQuery = GenerateQueryStringFromCategoryOptions();
        var response =
            await Client.GetAsync(
                $"https://localhost:5001/api/products/?SortBy=product_name&SortOrder={sortType}&{categoryQuery}&ProductTitleMatch={_searchTerm}");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var productList =
                JsonConvert.DeserializeObject<List<Product>>(JObject.Parse(responseJson)["items"].ToString());
            _items = productList.AsQueryable();
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }

    private async Task HandleSelectCategoryChange(List<SelectOption> selectedOptions)
    {
        await GetStoreProductsAsync();
    }


    private async Task OpenEditDialogAsync(Product context)
    {
        var dialog = await DialogService.ShowDialogAsync<CreateEditProductDialog>(context, new DialogParameters
        {
            Height = "400px",
            Title = $"Updating the {context.Name}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as Product;
            await UpdateProductAsync(item);
            await GetStoreProductsAsync();
        }
    }

    private async Task UpdateProductAsync(Product product)
    {
        var productJson = JsonConvert.SerializeObject(product);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(productJson, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"https://localhost:5001/api/products/{product.Id}", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Product successfully updated."
            : $"Failed to update the product. Status code: {response.StatusCode}");
    }

    private async Task OpenCreateDialogAsync()
    {
        var context = new Product()
        {
            CategoryId = Guid.Empty,
            Name = "",
            Characteristics = ""
        };
        var dialog = await DialogService.ShowDialogAsync<CreateEditProductDialog>(context, new DialogParameters()
        {
            Height = "400px",
            Title = $"Updating the {context.Name}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as Product;
            await PostProductAsync(item);
            await GetStoreProductsAsync();
        }
    }

    private async Task PostProductAsync(Product product)
    {
        var productJson = JsonConvert.SerializeObject(product);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(productJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:5001/api/products", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Product successfully saved."
            : $"Failed to save the product. Status code: {response.StatusCode}");
    }

    private async Task OpenDeleteDialog(Product context)
    {
        var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
        {
            Content = new MessageBoxContent
            {
                Title = "Warning",
                MarkupMessage = new MarkupString($"Are you sure you want to delete {context.Name}?"),
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
            await DeleteProductAsync(context.Id);
            await GetStoreProductsAsync();
        }
    }

    private async Task DeleteProductAsync(Guid guid)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.DeleteAsync($"https://localhost:5001/api/products/{guid}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Product successfully deleted.");
        }
        else
        {
            Console.WriteLine($"Failed to delete the product. Status code: {response.StatusCode}");
        }
    }

    private async Task PrintTable()
    {
        var printer = new TablePrinter<Product>(_items);
        var tableContent = printer.PrintTable();
        await IJS.InvokeVoidAsync("printComponent", tableContent, "Products");
    }
}