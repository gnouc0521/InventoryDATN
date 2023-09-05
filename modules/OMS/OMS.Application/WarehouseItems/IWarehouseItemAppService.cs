using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseItems.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseItems
{
    public interface IWarehouseItemAppService : IApplicationService
    {
        Task<int> Create(WarehouseItemCreateDto input);
        Task<PagedResultDto<WarehouseItemListDto>> GetAll(int id);
        Task<PagedResultDto<WarehouseItemListDto>> GetAllItemRoot(int idParent);
        Task<PagedResultDto<WarehouseItemListDto>> GetAllItemSub(int idParent);

        // cường 
        Task<PagedResultDto<WarehouseItemListDto>> GetAllBin(long warehouseId);

        Task<string> GetinfoBin(int Id);

        Task<int> Delete(int id);
        Task<long> Update(WarehouseItemListDto input);
        Task<PagedResultDto<WarehouseItemListDto>> GetAsync(EntityDto itemId);
        Task<WarehouseItemListDto> GetById(EntityDto itemId);

        Task<long> UpdateCount(int idParent);

        //Layout
        Task<PagedResultDto<WarehouseItemListDto>> GetAllItemOutLayout(int id);
        Task<PagedResultDto<WarehouseItemListDto>> GetAllItemInLayout(int id);
        Task<long> SaveLayOut(WarehouseItemListDto input);
        Task<string> GetCodeLocationBin(int Id);
    }
}
