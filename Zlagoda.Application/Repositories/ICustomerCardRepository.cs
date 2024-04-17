using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public interface ICustomerCardRepository
{
    Task<bool> CreateAsync(CustomerCard customerCard);
    Task<CustomerCard?> GetByIdAsync(Guid id);
    Task<IEnumerable<CustomerCard>> GetAllAsync(CustomerCardQueryParameters? parameters);
    Task<bool> UpdateAsync(CustomerCard customerCard);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}