using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public interface ISoldProductService
{
    Task<IEnumerable<SoldProduct>> GetSoldProductDetailsAsync(SoldProductQueryParameters? queryParameters);
}