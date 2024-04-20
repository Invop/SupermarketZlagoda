using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Data;

namespace SupermarketZlagoda.Components.Layout;

public partial class Login
{
    public string UserLogin { get; set; } = "";
    public string UserPassword { get; set; } = "";
    
    private EditContext _editContext = default!;
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
    
    private async Task SignInAsync()
    {
        
    }
    //
    // private async Task CancelAsync()
    // {
    //     
    // }
}
