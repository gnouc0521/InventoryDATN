using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Quotes
{
    public interface IQuotesService : IApplicationService
    {
        Task<long> Create(QuoteListDto input);
        Task<PagedResultDto<ItemsListDto>> GetAll(QuoteSearch input);
        Task<PagedResultDto<QuoteListDto>> GetHistoryDetail(QuoteSearch input);
        Task<QuoteListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(QuoteListDto input);
        Task<long> Delete(long id);
        Task<PagedResultDto<QuoteListDto>> GetAllHistory(QuoteSearch input);
        Task<PagedResultDto<QuoteListDto>> GetQuotebyItemsid(QuoteSearch input);

        //Ha Code
        Task<PagedResultDto<QuoteListDto>> GetHisByHa(QuoteSearch input);

        //kien code
        //Task<PagedResultDto<QuoteListDto>> GetAll();

    }
}
