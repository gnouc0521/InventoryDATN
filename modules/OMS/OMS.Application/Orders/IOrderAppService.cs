using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Orders.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task<long> Create(OrderListDto input);
        Task<PagedResultDto<OrderListDto>> GetAll(OrderDetailSearch input);
        Task<PagedResultDto<QuoteListDto>> GetAllDetail(OrderDetailSearch input);
        Task<PagedResultDto<QuoteListDto>> GetAssignment(OrderDetailSearch input);
        Task<OrderListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(OrderListDto input);
        Task<long> UpdateCreate(OrderListDto input);
        Task<long> Delete(long id);

        Task<bool> CheckContract(OrderDetailSearch input);

        //kien
        Task<long> UpdateStatus(OrderListDto input);
        Task<long> UpdateStatusSL(OrderListDto input);
        Task<PagedResultDto<QuoteListDto>> GetAllDetailSL(OrderDetailSearch input);

        //Code Ha
        //Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllItemByExpert(PurchasesSynthesisSearch input);


        //Task<PagedResultDto<PurchasesSynthesisListDto>> GetItemByStaff();
    }
}
