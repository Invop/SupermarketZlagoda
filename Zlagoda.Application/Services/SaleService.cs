using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;

namespace Zlagoda.Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;

    public SaleService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }
    
    public async Task<bool> CreateAsync(Sale sale)
    {
        return await _saleRepository.CreateAsync(sale);
    }

    public async Task<Sale?> GetByUPCCheckAsync(string upc, Guid sale)
    {
        return await _saleRepository.GetByUPCCheckAsync(upc, sale);
    }

    public async Task<IEnumerable<Sale>> GetSaleByIdCheckAsync(Guid sale)
    {
        return await _saleRepository.GetSaleByIdCheckAsync(sale);
    }
    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _saleRepository.GetAllAsync();
    }
    
    public async Task<bool> DeleteByUPCCheckAsync(Guid sale)
    {
        return await _saleRepository.DeleteByUPCCheckAsync(sale);
    }
}