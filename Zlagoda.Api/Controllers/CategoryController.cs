using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.Requests;

namespace Zlagoda.Api.Controllers;

public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpPost(ApiEndpoints.Categories.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        var category = request.MapToCategory();
        await categoryService.CreateAsync(category);
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }

    [HttpGet(ApiEndpoints.Categories.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var category = await categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category.MapToResponse());
    }
    
    [HttpGet(ApiEndpoints.Categories.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var categories = await categoryService.GetAllAsync();
        var categoriesResponse = categories.MapToResponse();
        return Ok(categoriesResponse);
    }


    [HttpPut(ApiEndpoints.Categories.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateCategoryRequest request)
    {
        var category = request.MapToCategory(id);
        var updatedCategory = await categoryService.UpdateAsync(category);
        if (updatedCategory is null)
        {
            return NotFound();
        }

        var response = category.MapToResponse();
        return Ok(response);
    }
    
    
    [HttpDelete(ApiEndpoints.Categories.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await categoryService.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}