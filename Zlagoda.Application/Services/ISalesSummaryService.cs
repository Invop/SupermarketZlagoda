using Zlagoda.Application.Models;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public interface ISalesSummaryService
{
    Task<IEnumerable<SaleSummary>> GetAll(SalesSummaryQueryParameters queryParameters);
}