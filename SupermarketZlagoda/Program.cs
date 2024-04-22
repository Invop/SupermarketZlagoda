using Blazored.LocalStorage;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Components;
using SupermarketZlagoda.Data;

namespace SupermarketZlagoda
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //SqliteDataAccess.InitDatabaseAndTables();

            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<User>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.Run();
        }
    }
}