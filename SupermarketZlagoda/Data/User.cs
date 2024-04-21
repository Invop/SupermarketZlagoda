using SupermarketZlagoda.Data.Model;
using System.Security.Cryptography;
using System.Text;

namespace SupermarketZlagoda.Data;

public static class User
{
    public static Employee? Data { get; set; } = null;

    public static bool Authorized { get; set; } = false;
    public static bool IsManager => Authorized && Data.Role.Equals("Manager");
}