﻿using System.Text;
using SupermarketZlagoda.Data.Model;

namespace SupermarketZlagoda.Data;

public class TablePrinter<T>(IEnumerable<T> items)
{
    public string PrintTable()
    {
        var sb = new StringBuilder();
        sb.Append("<html><head><title>Print</title>");
        sb.Append("<style>");
        sb.Append("table { border-collapse: collapse; width: 100%; }");
        sb.Append("th, td { border: 0.3px solid black; padding: 8px; text-align: left; }");
        sb.Append("th { background-color: #f2f2f2; }");
        sb.Append("</style>");
        sb.Append("</head><body><table>");

        var properties = typeof(T).GetProperties();
        sb.Append("<tr>");
        foreach (var prop in properties)
        {
            if (typeof(T) == typeof(Employee) && prop.Name is "UserLogin" or "UserPassword")
                continue;

            sb.Append($"<th>{prop.Name}</th>");
        }

        if (typeof(T) == typeof(Sale))
        {
            sb.Append("<th>Total Price</th>");
        }

        sb.Append("</tr>");

        foreach (var item in items)
        {
            sb.Append("<tr>");
            foreach (var prop in properties)
            {
                if (typeof(T) == typeof(Employee) && prop.Name is "UserLogin" or "UserPassword")
                    continue;

                sb.Append($"<td>{prop.GetValue(item)}</td>");
            }

            if (typeof(T) == typeof(Sale))
            {
                var sale = (Sale)(object)item;
                var totalPrice = sale.ProductNumber * sale.SellingPrice;
                sb.Append($"<td>{totalPrice}</td>");
            }

            sb.Append("</tr>");
        }

        sb.Append("</table></body></html>");
        return sb.ToString();
    }
}