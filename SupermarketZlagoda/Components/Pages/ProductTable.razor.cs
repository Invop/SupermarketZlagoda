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

public partial class ProductTable
{
    private bool IsManager { get; set; } = false;
    
    private string _searchTerm = string.Empty;
    private int _sortType = 0;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Product>? _items = Enumerable.Empty<Product>().AsQueryable();
    private List<SelectOption> CategoryOptions = new()
    {
        new() { Value = "1", Text = "Technology", Selected = true },
        new() { Value = "2", Text = "Science", Selected = true },
        new() { Value = "3", Text = "Art & Culture" },
        new() { Value = "4", Text = "Health & Wellness" },
        new() { Value = "5", Text = "Sports" },
        new() { Value = "6", Text = "Education" },
        // ... other category options
    };
    private static readonly HttpClient Client = new HttpClient();
    protected override async Task OnInitializedAsync()
    {
        IsManager = UserState.IsManager;
        await UpdateTable();
    }

    private async Task UpdateTable()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/products");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<List<Product>>(JObject.Parse(responseJson)["items"].ToString());
            _items = productList.AsQueryable();
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
    
    private void HandleSelectCategoryChange(List<SelectOption> selectedOptions)
    {
        foreach (var selectedOption in selectedOptions.Where(selectedOption => selectedOption.Selected))
        {
            Console.WriteLine(selectedOption.Value);
        }
    }
    

    private async Task OpenEditDialogAsync(Product context)
    {
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
                await UpdateProductAsync(item);
                await UpdateTable();
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
            CategoryId = 0,
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
            await UpdateTable();
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
            await UpdateTable();
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
}