using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.InventoryTicketDetails.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.InventoryTicketDetails
{
    public interface IInventoryTicketDetailService: IApplicationService
    {
        Task<PagedResultDto<InventoryTicketDetailListDto>> GetAll(GetInventoryTicketDetailInput input);
        Task<long> Create(InventoryTicketDetailCreateDto input);
        Task<long> Update(InventoryTicketDetailListDto input);
        Task<InventoryTicketDetailListDto> GetAsync(EntityDto itemId);
       
    }
}
