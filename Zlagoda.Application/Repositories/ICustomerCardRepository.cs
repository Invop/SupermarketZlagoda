using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface ICustomerCardRepository
{
    Task<bool> CreateAsync(CustomerCard customerCard);
    Task<CustomerCard?> GetByIdAsync(Guid id);
    Task<IEnumerable<CustomerCard>> GetAllAsync();
    Task<bool> UpdateAsync(CustomerCard customerCard);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}