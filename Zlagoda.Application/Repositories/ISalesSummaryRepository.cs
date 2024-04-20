using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Repositories;

public interface ISalesSummaryRepository
{
    Task<IEnumerable<SaleSummary>> GetAllAsync(SalesSummaryQueryParameters? queryParameters);
}