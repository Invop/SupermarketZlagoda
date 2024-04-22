using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Data;

public class User
{
    public Employee? Data { get; set; } = null;

    public bool Authorized { get; set; } = false;
    public bool IsManager { get; set; }
}