using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using bbk.netcore.mdl.OMS.Application.PurchaseAssignments.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchasesSynthesises
{
    public interface IPurchasesSynthesiseAppService : IApplicationService
    {
        Task<long> Create(PurchasesSynthesisListDto input);
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetAll(PurchasesSynthesisSearch input);
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllItems(PurchasesSynthesisSearch input);
        Task<PurchasesSynthesisListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(PurchasesSynthesisListDto input);
        Task<List<long>> Delete(long id);
     


        ///kien them
        Task<long> UpdateStatus(PurchasesSynthesisListDto input);
        Task<long> UpdateSyn(PurchasesSynthesisListDto input);
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllItemByExpert(PurchasesSynthesisSearch input);
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetItemByStaff();
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetViewCV(PurchasesSynthesisSearch input);
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllDone(PurchasesSynthesisSearch input);
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllConfirm(PurchasesSynthesisSearch input);
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetItemByStaffD();

        /// Cường 
        /// 
        Task<PagedResultDto<PurchasesSynthesisListDto>> GetItemsAssignments(PurchasesSynthesisSearch input);

        ///
        Task<FileDto> GetPurchaseAssignmentListDto(PurchasesSynthesisSearch input);

    }
}
