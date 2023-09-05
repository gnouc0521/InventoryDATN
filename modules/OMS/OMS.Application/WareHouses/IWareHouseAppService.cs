using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WareHouses
{
    public interface IWareHouseAppService : IApplicationService
    {
        Task<PagedResultDto<WarehouseListDto>> GetAll(GetWarehouseInput input);
        Task<WarehouseListDto> GetAsync(EntityDto<long> itemId);
        Task<int> Create(WarehouseCreateDto input);
        Task<int> Update(WarehouseEditDto input);
        Task<int> Delete(int id);
        Task<Address> GetAddress(string filePath, string superiorId);

        Task<List<WarehouseListDto>> GetWarehouseList();
        Task<PagedResultDto<WarehouseListDto>> GetAllExceptId(GetWarehouseInput input);

    }
}
