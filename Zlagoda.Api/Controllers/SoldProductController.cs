using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Application.Repositories;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.QueryParameters;
using Zlagoda.Contracts.Requests;
namespace Zlagoda.Api.Controllers;

[ApiController]
public class SoldProductController : ControllerBase
{
        private readonly ISoldProductService _soldProductService;

    public SoldProductController(ISoldProductService soldProductService)
    {
        _soldProductService = soldProductService;
    }


    [HttpGet(ApiEndpoints.SoldProducts.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] SoldProductQueryParameters? parameters)
    {
        var products = await _soldProductService.GetSoldProductDetailsAsync(parameters);
        var soldProductsResponse = products.MapToSoldProductsResponse();
        return Ok(soldProductsResponse);
    }


}