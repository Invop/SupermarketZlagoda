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

public partial class StoreProductTable
{
    private bool IsManager { get; set; } = true;
    
    private string _searchTerm = string.Empty;
    private int _sortType = 0;
    private bool _hidePromotional = false, _hideNonPromotional = false;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<StoreProduct>? _items = Enumerable.Empty<StoreProduct>().AsQueryable();
    private static readonly HttpClient Client = new HttpClient();
    protected override async Task OnInitializedAsync()
    {
        IsManager = UserState.IsManager;
        await UpdateTable();
    }
    
    private async Task UpdateTable()
    {
        var response = await Client.GetAsync("https://localhost:5001/api/store-products");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<List<StoreProduct>>(JObject.Parse(responseJson)["items"].ToString());
            if (productList != null) _items = productList.AsQueryable();
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }

    private async Task<StoreProduct?> GetProductByPromUpc(string upc)
    {
        var response = await Client.GetAsync($"https://localhost:5001/api/store-products/promo/{upc}");
    
        if (response is { IsSuccessStatusCode: true, Content: not null })
        {
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(responseJson))
            {
                var product = JsonConvert.DeserializeObject<StoreProduct>(responseJson);

                if (product != null)
                {
                    return product;
                }
            }
        } 
    
        Console.WriteLine($"Error: {response.StatusCode}");
        return null;
    }
    
    private async Task OpenCreateDialogAsync()
    {
        var context = new StoreProduct();
        var dialog = await DialogService.ShowDialogAsync<CreateEditStoreProductDialog>(context, new DialogParameters()
        {
            Height = "600px",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as StoreProduct;
            await PostStoreProductAsync(item);
            await UpdateTable();
        }
    }
    private async Task PostStoreProductAsync(StoreProduct product)
    {
        var productJson = JsonConvert.SerializeObject(product);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(productJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:5001/api/store-products", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Store Product successfully saved."
            : $"Failed to save the product. Status code: {response.StatusCode}");
    }

    private async Task OpenDeleteDialog(StoreProduct context)
    {
        var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
        {
            Content = new MessageBoxContent
            {
                Title = "Warning",
                MarkupMessage = new MarkupString($"Are you sure you want to delete {context.Upc}?"),
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
            if (context.IsPromotional)
            {
                var notPromoProduct = await GetProductByPromUpc(context.Upc);
                if (notPromoProduct != null)
                {
                    notPromoProduct.Quantity += context.Quantity;
                    await UpdateStoreProductAsync(notPromoProduct, notPromoProduct.Upc);
                }
            }

            await DeleteProductAsync(context.Upc);
            await UpdateTable();
        }
    }

    private async Task DeleteProductAsync(string? upc)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.DeleteAsync($"https://localhost:5001/api/store-products/{upc}");

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Product successfully deleted."
            : $"Failed to delete the product. Status code: {response.StatusCode}");
    }
    
    
    private async Task OpenEditDialogAsync(StoreProduct context)
    {
        var prevUpc = context.Upc;
        var dialog = await DialogService.ShowDialogAsync<CreateEditStoreProductDialog>(context, new DialogParameters()
        {
            Height = "400px",
            Title = $"Updating  {context.Upc}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });
        
        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as StoreProduct;
            if (item.IsPromotional)
            {
                var notPromoProduct = await GetProductByPromUpc(prevUpc);
                notPromoProduct.Quantity -= item.Quantity;
                await UpdateStoreProductAsync(notPromoProduct,notPromoProduct.Upc);
            }
            await UpdateStoreProductAsync(item,prevUpc);
            await UpdateTable();
        }
            
    }

    private async Task UpdateStoreProductAsync(StoreProduct product, string contextUpc)
    {
        var productJson = JsonConvert.SerializeObject(product);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(productJson, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"https://localhost:5001/api/store-products/{contextUpc}", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Product successfully updated."
            : $"Failed to update the product. Status code: {response.StatusCode}");
    }

    private async Task OpenAddPromoStoreProductDialog(StoreProduct context)
    {   
        StoreProduct promoStoreProduct = new StoreProduct
        {   
            ProductId = context.ProductId,
            Quantity = context.Quantity,
            Price = context.Price * 0.8m,
            IsPromotional = true,
        };
        var dialog = await DialogService.ShowDialogAsync<CreateEditStoreProductDialog>(promoStoreProduct, new DialogParameters()
        {
            Height = "400px",
            Title = $"Create new promo product",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });
        
        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as StoreProduct;
            context.UpcProm = item.Upc;
            context.Quantity -= item.Quantity;
            await UpdateStoreProductAsync(context,context.Upc);
            await PostStoreProductAsync(item);
            await UpdateTable();
        }
    }
}