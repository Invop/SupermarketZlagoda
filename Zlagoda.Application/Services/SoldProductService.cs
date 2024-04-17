using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public class SoldProductService : ISoldProductService
{   
    
    private readonly ISoldProductRepository _soldProductRepository;

    public SoldProductService(ISoldProductRepository soldProductRepository)
    {
        _soldProductRepository = soldProductRepository;
    }

    public Task<IEnumerable<SoldProduct>> GetSoldProductDetailsAsync(SoldProductQueryParameters? queryParameters)
    {
        return _soldProductRepository.GetSoldProductDetailsAsync(queryParameters);
    }
}