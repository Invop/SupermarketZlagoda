using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public interface ICheckService
{
    Task<bool> CreateAsync(Check check);
    Task<Check?> GetByIdAsync(Guid id);
    Task<IEnumerable<Check>> GetAllAsync(CheckQueryParameters? parameters);
    Task<bool> DeleteByIdAsync(Guid id);
}