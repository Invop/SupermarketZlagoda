using Microsoft.AspNetCore.Components;

namespace SupermarketZlagoda.Components.Forms;

public partial class LoginForm
{
    private string? _roleType;

    protected override void OnInitialized()
    {
    }

    [SupplyParameterFromForm]
    private Data.Model.User _newUser { get; set; } = new();

    private void HandleValidSubmit()
    {
        Console.WriteLine("HandleValidSubmit called");

    }
}