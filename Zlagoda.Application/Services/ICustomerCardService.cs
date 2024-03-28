using Zlagoda.Application.Models;

namespace Zlagoda.Application.Services;

public interface ICustomerCardService
{
    Task<bool> CreateAsync(CustomerCard customerCard);
    Task<CustomerCard?> GetByIdAsync(Guid id);
    Task<IEnumerable<CustomerCard>> GetAllAsync();
    Task<CustomerCard?> UpdateAsync(CustomerCard customerCard);
    Task<bool> DeleteByIdAsync(Guid id);
}