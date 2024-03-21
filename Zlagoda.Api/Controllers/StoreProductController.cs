using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Application.Repositories;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.Requests;

namespace Zlagoda.Api.Controllers;

[ApiController]
public class StoreProductController : ControllerBase
{
    private readonly IStoreProductService _productService;

    public StoreProductController(IStoreProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost(ApiEndpoints.StoreProducts.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var product = request.MapToProduct();
        await _productService.CreateAsync(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpGet(ApiEndpoints.StoreProducts.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product.MapToResponse());
    }
    
    [HttpGet(ApiEndpoints.StoreProducts.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        var productsResponse = products.MapToResponse();
        return Ok(productsResponse);
    }


    [HttpPut(ApiEndpoints.StoreProducts.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateProductRequest request)
    {
        var product = request.MapToProduct(id);
        var updatedProduct = await _productService.UpdateAsync(product);
        if (updatedProduct is null)
        {
            return NotFound();
        }

        var response = product.MapToResponse();
        return Ok(response);
    }
    
    
    [HttpDelete(ApiEndpoints.StoreProducts.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _productService.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}