using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;

namespace Zlagoda.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<bool> CreateAsync(Category category)
    {
        return await _categoryRepository.CreateAsync(category);
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category?> UpdateAsync(Category category)
    {
        var CategoryExists = await _categoryRepository.ExistsByIdAsync(category.Id);
        if (!CategoryExists)
        {
            return null;
        }
        await _categoryRepository.UpdateAsync(category);
        return category;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await _categoryRepository.DeleteByIdAsync(id);
    }
}