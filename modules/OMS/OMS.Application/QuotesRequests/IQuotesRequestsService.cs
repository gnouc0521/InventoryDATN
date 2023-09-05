using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.QuotesRequests.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.QuotesRequests
{
    public interface IQuotesRequestsService : IApplicationService
    {
        Task<long> Create(QuoteRequestsListDto input);
        Task<PagedResultDto<ItemsListDto>> GetAll(QuoteRequestsSearch input);
        Task<PagedResultDto<QuoteRequestsListDto>> GetHistoryDetail(QuoteRequestsSearch input);
        Task<QuoteRequestsListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(QuoteRequestsListDto input);
        Task<long> Delete(long id);
        // delete when update quotesyn
        Task<long> DeleteQuoteRequests(QuoteRequestsListDto input);
        Task<PagedResultDto<QuoteRequestsListDto>> GetAllHistory(QuoteRequestsSearch input);
        Task<PagedResultDto<QuoteRequestsListDto>> GetQuotebyItemsid(QuoteRequestsSearch input);

        //Ha Code
        Task<PagedResultDto<QuoteRequestsListDto>> GetHisByHa(QuoteRequestsSearch input);

    }
}
