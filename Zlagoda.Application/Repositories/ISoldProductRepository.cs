using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public interface ISoldProductRepository
{
    Task<IEnumerable<SoldProduct>> GetSoldProductDetailsAsync(SoldProductQueryParameters? queryParameters);
}