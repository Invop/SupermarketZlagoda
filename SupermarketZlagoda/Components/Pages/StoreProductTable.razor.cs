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
            await UpdateTable();
        }
    }

    
    
    private void HandleSelectCategoryChange(List<SelectOption> selectedOptions)
    {
        foreach (var selectedOption in selectedOptions.Where(selectedOption => selectedOption.Selected))
        {
            Console.WriteLine(selectedOption.Value);
        }
    }
}