﻿using Zlagoda.Application.Models;

namespace Zlagoda.Application.Repositories;

public interface ISaleRepository
{
    Task<bool> CreateAsync(Sale sale);
    Task<Sale?> GetByUPCCheckAsync(string upc, Guid check);
    Task<IEnumerable<Sale>> GetSaleByIdCheckAsync(Guid sale);
    Task<IEnumerable<Sale>> GetAllAsync();
    Task<bool> DeleteByUPCCheckAsync(Guid check);
 
}