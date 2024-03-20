using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Application.Repositories;
using Zlagoda.Contracts.Requests;

namespace Zlagoda.Api.Controllers;

[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    [HttpPost(ApiEndpoints.Products.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var product = request.MapToProduct();
        await _productRepository.CreateAsync(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpGet(ApiEndpoints.Products.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid guid)
    {
        var product = await _productRepository.GetByIdAsync(guid);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product.MapToResponse());
    }
    
    [HttpGet(ApiEndpoints.Products.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        var productsResponse = products.MapToResponse();
        return Ok(productsResponse);
    }


    [HttpPut(ApiEndpoints.Products.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateProductRequest request)
    {
        var product = request.MapToProduct(id);
        var updated = await _productRepository.UpdateAsync(product);
        if (!updated)
        {
            return NotFound();
        }

        var response = product.MapToResponse();
        return Ok(response);
    }
    
    
    [HttpDelete(ApiEndpoints.Products.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _productRepository.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}