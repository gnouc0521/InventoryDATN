using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.QuotesSynthesises.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchasesSynthesises
{
   public  interface IQuotesSynthesiseAppService : IApplicationService
    {
        Task<long> Create(QuotesSynthesisListDto input);
        Task<PagedResultDto<QuotesSynthesisListDto>> GetAllByCreator(QuotesSynthesisSearch input);
        Task<PagedResultDto<QuotesSynthesisListDto>> GetAll(QuotesSynthesisSearch input);
       Task<PagedResultDto<QuoteListDto>> Detail(QuotesSynthesisSearch input);
        Task<QuotesSynthesisListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(QuotesSynthesisListDto input);
        Task<List<long>> GetQuoteId(QuotesSynthesisListDto input);
        Task<long> Delete(long id);

        //Ha Code
        Task<PagedResultDto<QuotesSynthesisListDto>> GetAllQuoteApprove(QuotesSynthesisSearch input);
        Task<QuotesSynthesisListDto> GetQuoteApprove(int Id);
        Task<long> UpdateStatus(QuotesSynthesisListDto input);

    }
}
