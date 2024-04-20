using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Application.Repositories;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.QueryParameters;
using Zlagoda.Contracts.Requests;

namespace Zlagoda.Api.Controllers;

[ApiController]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly ISalesSummaryRepository _salesSummary;

    public SaleController(ISaleService saleService, ISalesSummaryRepository salesSummary)
    {
        _saleService = saleService;
        _salesSummary = salesSummary;
    }

    [HttpPost(ApiEndpoints.Sales.Create)]
    public async Task<IActionResult> Create([FromBody] CreateSaleRequest request)
    {
        var sale = request.MapToSale();
        await _saleService.CreateAsync(sale);
        return CreatedAtAction(nameof(Get), new { upc = sale.Upc, check = sale.CheckNumber }, sale);
    }

    [HttpGet(ApiEndpoints.Sales.Get)]
    public async Task<IActionResult> Get([FromRoute] string upc, [FromRoute] Guid check)
    {
        var sale = await _saleService.GetByUpcCheckAsync(upc, check);
        if (sale == null)
        {
            return NotFound();
        }

        return Ok(sale.MapToSaleResponse());
    }

    [HttpGet(ApiEndpoints.Sales.GetById)]
    public async Task<IActionResult> GetById([FromRoute] Guid check)
    {
        var sales = await _saleService.GetSaleByIdCheckAsync(check);
        var salesResponse = sales.MapToSalesResponse();
        return Ok(salesResponse);
    }

    [HttpGet(ApiEndpoints.Sales.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _saleService.GetAllAsync();
        var salesResponse = sales.MapToSalesResponse();
        return Ok(salesResponse);
    }


    [HttpGet(ApiEndpoints.Sales.GetSummary)]
    public async Task<IActionResult> GetSummary([FromQuery] SalesSummaryQueryParameters? parameters)
    {
        var sales = await _salesSummary.GetAllAsync(parameters);
        var salesResponse = sales.MapToSalesSummaryResponse();
        return Ok(salesResponse);
    }


    [HttpDelete(ApiEndpoints.Sales.Delete)]
    public async Task<IActionResult> Delete(Guid check)
    {
        var deleted = await _saleService.DeleteByUpcCheckAsync(check);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}