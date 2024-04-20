using Zlagoda.Application.Models;
using Zlagoda.Application.Repositories;
using Zlagoda.Contracts.QueryParameters;

namespace Zlagoda.Application.Services;

public class SalesSummaryService : ISalesSummaryService
{
    private readonly ISalesSummaryRepository _salesSummaryRepository;

    public SalesSummaryService(ISalesSummaryRepository salesSummaryRepository)
    {
        _salesSummaryRepository = salesSummaryRepository;
    }

    public async Task<IEnumerable<SaleSummary>> GetAll(SalesSummaryQueryParameters queryParameters)
    {
        var saleSummaries = await _salesSummaryRepository.GetAllAsync(queryParameters);
        return saleSummaries;
    }
}