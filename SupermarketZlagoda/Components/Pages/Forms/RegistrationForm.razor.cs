using Microsoft.AspNetCore.Components;

namespace SupermarketZlagoda.Components.Pages.Forms;

public partial class RegistrationForm
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