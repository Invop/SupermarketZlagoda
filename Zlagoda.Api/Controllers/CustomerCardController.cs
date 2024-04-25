using Microsoft.AspNetCore.Mvc;
using Zlagoda.Api.Mapping;
using Zlagoda.Application.Services;
using Zlagoda.Contracts.QueryParameters;
using Zlagoda.Contracts.Requests;

namespace Zlagoda.Api.Controllers;

[ApiController]
public class CustomerCardController : ControllerBase
{
    private readonly ICustomerCardService _customerCardService;

    public CustomerCardController(ICustomerCardService customerCardService)
    {
        _customerCardService = customerCardService;
    }

    [HttpPost(ApiEndpoints.CustomerCards.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCardRequest request)
    {
        var customerCard = request.MapToCustomerCard();
        await _customerCardService.CreateAsync(customerCard);
        return CreatedAtAction(nameof(Get), new { id = customerCard.Id }, customerCard);
    }

    [HttpGet(ApiEndpoints.CustomerCards.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var customerCard = await _customerCardService.GetByIdAsync(id);
        if (customerCard == null)
        {
            return NotFound();
        }

        return Ok(customerCard.MapToProductResponse());
    }

    [HttpGet(ApiEndpoints.CustomerCards.GetZapitData)]
    public async Task<IActionResult> GetAll()
    {
        var customerCards = await _customerCardService.GetZapitDataAsync();
        var customerCardsResponse = customerCards.MapToProductResponse();
        return Ok(customerCardsResponse);
    }

    [HttpGet(ApiEndpoints.CustomerCards.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] CustomerCardQueryParameters? parameters)
    {
        var customerCards = await _customerCardService.GetAllAsync(parameters);
        var customerCardsResponse = customerCards.MapToProductResponse();
        return Ok(customerCardsResponse);
    }


    [HttpPut(ApiEndpoints.CustomerCards.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateCustomerCardRequest request)
    {
        var customerCard = request.MapToCustomerCard(id);
        var updatedCustomerCard = await _customerCardService.UpdateAsync(customerCard);
        if (updatedCustomerCard is null)
        {
            return NotFound();
        }

        var response = customerCard.MapToProductResponse();
        return Ok(response);
    }


    [HttpDelete(ApiEndpoints.CustomerCards.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _customerCardService.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}