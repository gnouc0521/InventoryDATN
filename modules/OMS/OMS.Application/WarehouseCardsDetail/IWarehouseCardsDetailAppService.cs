using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseCardsDetail.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseCardsDetail
{
    public interface IWarehouseCardsDetailAppService : IApplicationService
    {
        Task<long> Create(WarehouseCardsDetailCreate input);
        Task<long> Update(WarehouseCardsDetailCreate input);
        Task<long> Delete(long id);


    }
}
