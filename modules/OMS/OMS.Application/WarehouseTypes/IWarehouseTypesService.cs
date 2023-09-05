using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseTypes.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseTypes
{
    public interface IWarehouseTypesService : IApplicationService
    {
        Task<PagedResultDto<WarehouseTypeListDto>> GetAll(WarehouseTypeSearch input);
        Task<WarehouseTypeListDto> GetAsync(EntityDto itemId);
        Task<int> Create(WarehouseTypeListDto input);
        Task<int> Update(WarehouseTypeListDto input);
        Task<int> Delete(int id);

        //Hà thêm
        Task<List<WarehouseTypeListDto>> GetAllWarehouseType();
    }
}
