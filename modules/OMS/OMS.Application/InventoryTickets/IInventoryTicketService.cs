using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Inventorys.Dto;
using bbk.netcore.mdl.OMS.Application.InventoryTickets.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.InventoryTickets
{
    public interface IInventoryTicketService : IApplicationService
    {
        Task<PagedResultDto<InventoryTicketListDto>> GetAll(GetInventoryTicketInput input);
        Task<long> Create(InventoryTicketCreateDto input);
        Task<long> Update(InventoryTicketListDto input);
        Task<InventoryTicketListDto> GetAsync(EntityDto itemId);
    }
}
