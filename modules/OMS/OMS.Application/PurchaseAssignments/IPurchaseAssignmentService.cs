using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using bbk.netcore.mdl.OMS.Application.PurchaseAssignments.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchaseAssignments
{
    public interface IPurchaseAssignmentService : IApplicationService
    {
        Task<long> Create(PurchaseAssignmentCreateDto input);

        Task<PurchaseAssigmentListDto> GetAsync(EntityDto itemId);

        Task<long> Update(PurchaseAssigmentListDto input);

        Task<long> UpdateUserId(PurchaseAssigmentListDto input);

     
    }
}
