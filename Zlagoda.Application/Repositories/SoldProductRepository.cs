using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Zlagoda.Application.Database;
using Zlagoda.Application.Models;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public class SoldProductRepository : ISoldProductRepository
{   
    
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public SoldProductRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    private SqlCommand GetCommandWithParameters(SoldProductQueryParameters parameters, SqlCommand command)
    {
        if (parameters.CashierId != Guid.Empty && parameters.CashierId != null)
        {
            command.Parameters.Add(new SqlParameter("@CashierId", SqlDbType.UniqueIdentifier) { Value = parameters.CashierId });
        }

        if (parameters.StartDate != null)
        {
            command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = parameters.StartDate });
        }

        if (parameters.EndDate != null)
        {
            command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = parameters.EndDate });
        }

        return command;
    }

    private void AppendAdditionalCriteria(StringBuilder commandText, SoldProductQueryParameters? parameters)
    {
        if (parameters == null) return;

        if (parameters.CashierId != Guid.Empty && parameters.CashierId != null)
        {
            commandText.Append(" AND e.id_employee = @CashierId");
        }

        if (parameters.StartDate != null && parameters.EndDate != null)
        {
            commandText.Append(" AND c.print_date BETWEEN @StartDate AND @EndDate");
        }
        else if (parameters.StartDate != null)
        {
            commandText.Append(" AND c.print_date >= @StartDate");
        }
        else if (parameters.EndDate != null)
        {
            commandText.Append(" AND c.print_date <= @EndDate");
        }
    }

    public async Task<IEnumerable<SoldProduct>> GetSoldProductDetailsAsync(SoldProductQueryParameters? parameters)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var commandText = new StringBuilder("""
                                            
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
                                                
                                            """);

        AppendAdditionalCriteria(commandText, parameters);

        commandText.Append("""
                           
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
                                       selling_price;
                               
                           """);

        using var command = new SqlCommand(commandText.ToString(), connection);
        if(parameters != null)
        {
            GetCommandWithParameters(parameters, command);
        }

        using var reader = await command.ExecuteReaderAsync();
        var soldProducts = new List<SoldProduct>();
        while (await reader.ReadAsync())
        {
            var soldProduct = new SoldProduct
            {
                ProductId = reader.GetGuid(0),
                ProductName = reader.GetString(1),
                SellingPrice = reader.GetDecimal(2),
                TotalQuantity = reader.GetInt32(3)
            };
            soldProducts.Add(soldProduct);
        }

        return soldProducts;
    }
}