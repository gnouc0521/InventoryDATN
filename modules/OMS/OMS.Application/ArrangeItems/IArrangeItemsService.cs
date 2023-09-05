using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ArrangeItems.Dto;
using bbk.netcore.mdl.OMS.Application.DayOffs.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ArrangeItems
{
    public interface IArrangeItemsService : IApplicationService
    {
        Task<PagedResultDto<ArrangeItemsListDto>> GetAll();
    }
}
