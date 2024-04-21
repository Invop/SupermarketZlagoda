using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupermarketZlagoda.Data;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Layout;

public partial class Login
{
    [Required]
    public string UserLogin { get; set; } = "";
    
    [Required]
    public string UserPassword { get; set; } = "";
    
    private EditContext _editContext = default!;
    private static readonly HttpClient Client = new();
    private bool IsButtonDisabled { get; set; } = false;

    
    private async Task SignInAsync()
    {
        IsButtonDisabled = true;
        var response = await Client.GetAsync($"https://localhost:5001/api/employees/?UserLogin={UserLogin}&UserPassword={Employee.Hash(UserPassword)}");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return;
        }
        var content = await response.Content.ReadAsStringAsync();
        var list = JsonConvert.DeserializeObject<List<Employee>>(JObject.Parse(content)["items"].ToString());
        if (list.IsNullOrEmpty())
        {
            await DialogService.ShowErrorAsync("Wrong login or password!");
            IsButtonDisabled = false;
            return;
        }
        User.Data = list[0];
        User.Authorized = true;
        NavigationManager.NavigateTo("/");
        IsButtonDisabled = false;
    }
    
    
    //
    // private async Task CancelAsync()
    // {
    //     
    // }
}
