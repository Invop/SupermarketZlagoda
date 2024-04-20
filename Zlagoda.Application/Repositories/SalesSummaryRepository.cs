using System.Data;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public class SalesSummaryRepository : ISalesSummaryRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SalesSummaryRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<SaleSummary>> GetAllAsync(SalesSummaryQueryParameters? queryParameters)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var command = connection.CreateCommand();

        command.CommandText = @"
                WITH CTE AS (
                SELECT
                    sp.id_product,
                    p.product_name,
                    s.selling_price,
                    SUM(s.product_number) AS total_quantity
                FROM
                    Sales s
                    INNER JOIN Checks c ON s.check_number = c.check_number
                    INNER JOIN Store_Products sp ON s.UPC = sp.UPC
                    INNER JOIN Employees e ON c.id_employee = e.id_employee
                    INNER JOIN Products p ON sp.id_product = p.id_product
                WHERE 1=1
                    AND (@EmployeeId IS NULL OR e.id_employee = @EmployeeId)    
                    AND (@ProductId IS NULL OR sp.id_product = @ProductId)
                    AND (@StartDate IS NULL OR c.print_date >= @StartDate)
                    AND (@EndDate IS NULL OR c.print_date <= @EndDate)
                GROUP BY
                    sp.id_product,
                    p.product_name,
                    s.selling_price
                )
                SELECT
                    id_product,
                    product_name,
                    selling_price,
                    SUM(total_quantity) AS total_quantity
                FROM
                    CTE
                GROUP BY
                    id_product,
                    product_name,
                    selling_price";

        command.Parameters.Add("@EmployeeId", SqlDbType.UniqueIdentifier).Value =
            queryParameters.EmployId ?? (object)DBNull.Value;
        command.Parameters.Add("@ProductId", SqlDbType.UniqueIdentifier).Value =
            queryParameters.ProductId ?? (object)DBNull.Value;
        command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value =
            queryParameters.PeriodFrom ?? (object)DBNull.Value;
        command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = queryParameters.PeriodTo ?? (object)DBNull.Value;

        var soldProducts = new List<SaleSummary>();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            soldProducts.Add(new SaleSummary
            {
                ProductName = reader.GetString(1),
                SellingPrice = reader.GetDecimal(2),
                TotalQuantity = reader.GetInt32(3)
            });
        return soldProducts;
    }
}