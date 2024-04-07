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
        var product = await _productService.GetByUpcAsync(upc);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product.MapToStoreProductResponse());
    }
    [HttpGet(ApiEndpoints.StoreProducts.GetByPromoUpc)]
    public async Task<IActionResult> GetByPromoUpc([FromRoute] string upc)
    {
        var product = await _productService.GetByPromoUpcAsync(upc);
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
    [HttpGet(ApiEndpoints.StoreProducts.GetAllPromo)]
    public async Task<IActionResult> GetAllPromo()
    {
        var products = await _productService.GetAllPromoStoreProductsAsync();
        var productsResponse = products.MapToStoreProductResponse();
        return Ok(productsResponse);
    }
    
    [HttpGet(ApiEndpoints.StoreProducts.GetQuantityByUpcProm)]
    public async Task<IActionResult> GetQuantityByUpcProm([FromRoute] string upcProm)
    {
        var productQuantity = await _productService.GetQuantityByUpcPromAsync(upcProm);
        return Ok(productQuantity);
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
        var deleted = await _productService.DeleteByUpcAsync(upc);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}