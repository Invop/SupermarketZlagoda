using SupermarketZlagoda.Data.Model;
using System.Security.Cryptography;
using System.Text;

namespace SupermarketZlagoda.Data;

public class User
{
    public Employee? Data { get; set; } = null;

    public bool Authorized { get; set; } = false;
}