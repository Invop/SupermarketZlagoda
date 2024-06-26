﻿using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public class CustomerCardService : ICustomerCardService
{
    private readonly ICustomerCardRepository _customerCardRepository;

    public CustomerCardService(ICustomerCardRepository customerCardRepository)
    {
        _customerCardRepository = customerCardRepository;
    }

    public async Task<bool> CreateAsync(CustomerCard customerCard)
    {
        return await _customerCardRepository.CreateAsync(customerCard);
    }

    public async Task<CustomerCard?> GetByIdAsync(Guid id)
    {
        return await _customerCardRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<CustomerCard>> GetAllAsync(CustomerCardQueryParameters? parameters)
    {
        return await _customerCardRepository.GetAllAsync(parameters);
    }

    public async Task<CustomerCard?> UpdateAsync(CustomerCard customerCard)
    {
        var customerCardExists = await _customerCardRepository.ExistsByIdAsync(customerCard.Id);
        if (!customerCardExists)
        {
            return null;
        }

        await _customerCardRepository.UpdateAsync(customerCard);
        return customerCard;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await _customerCardRepository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<CustomerCard>> GetZapitDataAsync()
    {
        return await _customerCardRepository.GetZapitDataAsync();
    }
}