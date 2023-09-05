using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.Orders.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.OrdersDetail
{
    public interface IOrdersDetailAppService : IApplicationService
    {
        Task<long> Create(OrdersDetailListDto input);
        Task<PagedResultDto<OrdersDetailListDto>> GetAll(OrdersDetailSearch input);
        Task<OrdersDetailListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(OrdersDetailListDto input);
        Task<long> Delete(long id);


        //code kie
        Task<PagedResultDto<OrdersDetailListDto>> GetItemMission();
        Task<PagedResultDto<OrdersDetailListDto>> GetYCNK();
        Task<PagedResultDto<OrdersDetailListDto>> GetAllDetail(OrdersDetailSearch input);

        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="ordersDetailListDto">Id order </param>
        /// <returns> Export file execl </returns>
        Task<FileDto> GetPOListDto(OrdersDetailListDto ordersDetailListDto);
        //Code Ha
        //Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllItemByExpert(PurchasesSynthesisSearch input);


        //Task<PagedResultDto<PurchasesSynthesisListDto>> GetItemByStaff();
    }
}
