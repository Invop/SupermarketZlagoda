using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;

namespace Zlagoda.Application.Services;

public class CheckService : ICheckService
{
    private readonly ICheckRepository _checkRepository;

    public CheckService(ICheckRepository checkRepository)
    {
        _checkRepository = checkRepository;
    }
    
    public async Task<bool> CreateAsync(Check check)
    {
        return await _checkRepository.CreateAsync(check);
    }

    public async Task<Check?> GetByIdAsync(Guid id)
    {
        return await _checkRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Check>> GetAllAsync()
    {
        return await _checkRepository.GetAllAsync();
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await _checkRepository.DeleteByIdAsync(id);
    }
}