using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using SupermarketZlagoda.Data.Model;
namespace SupermarketZlagoda.Components.Pages;

public partial class EmployeeTable
{
    private int _sortType = 0;
    private bool _cashiersOnly = false;
    private string _surnameSearchTerm = string.Empty;
    private readonly PaginationState _pagination = new() { ItemsPerPage = 20 };
    private IQueryable<Employee>? _items = Enumerable.Empty<Employee>().AsQueryable();
    private IQueryable<Employee> GenerateSampleGridData(int size)
    {
        Employee[] data = new Employee[size];

        for (int i = 0; i < size; i++)
        {
            data[i] = new Employee(i, $"Surname looooooooooooooooong {i}", $"Name loooooooong {i}",
                $"Patronymic {i}", $"Role {i}", i * 100m,
                new DateOnly(2000, 1, 1), new DateOnly(2000, 1, 1),
                $"+380555{i}", $"City looooooooooooooooong {i}", $"Street looooooooooooooooong {i}", $"Code {i}");
        }
        return data.AsQueryable();
    }
    protected override void OnInitialized()
    {
        _items = GenerateSampleGridData(5000);
    }
}