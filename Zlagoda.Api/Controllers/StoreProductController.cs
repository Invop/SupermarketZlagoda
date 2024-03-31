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
    public async Task<IActionResult> Create([FromBody] CreateStoreProductRequest request)
    {
        var product = request.MapToStoreProduct();
        await _productService.CreateAsync(product);
        return CreatedAtAction(nameof(Get), new { upc = product.Upc }, product);
    }

    [HttpGet(ApiEndpoints.StoreProducts.Get)]
    public async Task<IActionResult> Get([FromRoute] string upc)
    {
        var product = await _productService.GetByUPCAsync(upc);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product.MapToStoreProductResponse());
    }
    
    [HttpGet(ApiEndpoints.StoreProducts.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        var productsResponse = products.MapToStoreProductResponse();
        return Ok(productsResponse);
    }
    [HttpGet(ApiEndpoints.StoreProducts.GetAllNotPromoUpc)]
    public async Task<IActionResult> GetAllNotPromoUpc()
    {
        var products = await _productService.GetAllNotPromoUpc();
        return Ok(products);
    }

    [HttpPut(ApiEndpoints.StoreProducts.Update)]
    public async Task<IActionResult> Update([FromRoute] string prevUpc,
        [FromBody] UpdateStoreProductRequest request)
    {
        var product = request.MapToStoreProduct();
        var updatedProduct = await _productService.UpdateAsync(product,prevUpc);
        if (updatedProduct is null)
        {
            return NotFound();
        }

        var response = product.MapToStoreProductResponse();
        return Ok(response);
    }
    
    
    [HttpDelete(ApiEndpoints.StoreProducts.Delete)]
    public async Task<IActionResult> Delete([FromRoute] string upc)
    {
        var deleted = await _productService.DeleteByUPCAsync(upc);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}