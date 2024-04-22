using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Components.Layout;

public partial class Login
{
    [Required] private string UserLogin { get; set; } = "";

    [Required] private string UserPassword { get; set; } = "";


    private static readonly HttpClient Client = new();


    private async Task SignInAsync()
    {
        var hashedPassword = HashPassword(UserPassword);
        var response =
            await Client.GetAsync(
                $"https://localhost:5001/api/employees/?UserLogin={UserLogin}&UserPassword={hashedPassword}");

        // Checking if the request was successful
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
            return;
        }

        User.Data = list?[0];
        User.Authorized = true;
        User.IsManager = User.Data?.Role == "Manager";
        await localStorage.SetItemAsync("UserData", User.Data);
        await localStorage.SetItemAsync("Authorized", User.Authorized);
        await localStorage.SetItemAsync("IsManager", User.IsManager);
        NavigationManager.NavigateTo("/", true);
    }

    private static string HashPassword(string value)
    {
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        var builder = new StringBuilder();
        foreach (var t in hashedBytes)
            builder.Append(t.ToString("x2"));
        return builder.ToString();
    }
}