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

public partial class CategoryTable
{
    private int _sortType = 0;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Category> _items = Enumerable.Empty<Category>().AsQueryable();
    private static readonly HttpClient Client = new HttpClient();
    protected override async Task OnInitializedAsync()
    {
        await GetCategoriesAsync();
    }
    private int SortType
    {
        get => _sortType;
        set { _sortType = value;
            _ = GetCategoriesAsync();
        }
    }

    private async Task GetCategoriesAsync()
    {   
        var sortType = _sortType == 0 ? "asc" : "desc";
        var response = await Client.GetAsync($"https://localhost:5001/api/categories/?SortBy=category_name&SortOrder={sortType}");
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<Category>>(JObject.Parse(responseJson)["items"].ToString());
            if (categories != null) _items = categories.AsQueryable();
            StateHasChanged();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
    
    private async Task OpenCreateDialogAsync()
    {
        var context = new Category { Name = "" };
        var dialog = await DialogService.ShowDialogAsync<CreateEditCategoryDialog>(context, new DialogParameters
        {
            Title = "Add new category",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as Category;
            await PostCategoryAsync(item);
            await GetCategoriesAsync();
        }
    }

    private async Task UpdateCategoryAsync(Category category)
    {
        var categoryJson = JsonConvert.SerializeObject(category);
        
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(categoryJson, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"https://localhost:5001/api/categories/{category.Id}", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Category successfully updated."
            : $"Failed to update the category. Status code: {response.StatusCode}");
    }
    
    private async Task OpenEditDialogAsync(Category context)
    {
        var dialog = await DialogService.ShowDialogAsync<CreateEditCategoryDialog>(context, new DialogParameters()
        {
            Title = $"Updating category {context.Name}",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            var item = result.Data as Category;
            await UpdateCategoryAsync(item);
            await GetCategoriesAsync();
        }  
    }
    private async Task PostCategoryAsync(Category category)
    {
        var categoryJson = JsonConvert.SerializeObject(category);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(categoryJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:5001/api/categories", content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Category successfully saved."
            : $"Failed to save the category. Status code: {response.StatusCode}");
    }
    
    private async Task OpenDeleteDialogAsync(Category category)
    {
        var dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
        {
            Content = new MessageBoxContent
            {
                Title = "Warning",
                MarkupMessage = new MarkupString($"Are you sure you want to delete {category.Name}?"),
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
            await DeleteCategoryAsync(category.Id);
            await GetCategoriesAsync();
        }
    }

    private async Task DeleteCategoryAsync(Guid guid)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.DeleteAsync($"https://localhost:5001/api/categories/{guid}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Category successfully deleted.");
        }
        else
        {
            Console.WriteLine($"Failed to delete the category. Status code: {response.StatusCode}");
        }
    }
    private async Task PrintTable()
    {
        var printer = new TablePrinter<Category>(_items);
        var tableContent = printer.PrintTable();
        await IJS.InvokeVoidAsync("printComponent", tableContent,"Categories");
    }
}