using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.QuotesRelationships.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.QuotesRelationships
{
    public interface IQuotesRelationshipService : IApplicationService
    {
        Task<long> Create(QuotesRelationshipListDto input);
        //Task<PagedResultDto<ItemsListDto>> GetAll(QuoteSearch input);
      //  Task<QuoteListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(QuotesRelationshipListDto input);
        Task<long> Delete(long id);

        //Task<PagedResultDto<QuoteListDto>> GetQuotebyItemsid(QuoteSearch input);


    }
}
