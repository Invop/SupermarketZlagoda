using Zlagoda.Application.Models;
namespace Zlagoda.Application.Services;

public interface ISaleService
{
    Task<bool> CreateAsync(Sale sale);
    Task<Sale?> GetByUPCCheckAsync(string upc, Guid sale);
    Task<IEnumerable<Sale>> GetSaleByIdCheckAsync(Guid check);
    Task<IEnumerable<Sale>> GetAllAsync();
    Task<bool> DeleteByUPCCheckAsync(Guid sale);
}