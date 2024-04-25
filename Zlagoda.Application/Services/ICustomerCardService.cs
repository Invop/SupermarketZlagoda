using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public interface ICustomerCardService
{
    Task<bool> CreateAsync(CustomerCard customerCard);
    Task<CustomerCard?> GetByIdAsync(Guid id);
    Task<IEnumerable<CustomerCard>> GetAllAsync(CustomerCardQueryParameters? parameters);
    Task<CustomerCard?> UpdateAsync(CustomerCard customerCard);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<IEnumerable<CustomerCard>> GetZapitDataAsync();
}