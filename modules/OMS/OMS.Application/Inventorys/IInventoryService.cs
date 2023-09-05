using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Inventorys.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Inventorys
{
    public interface IInventoryService : IApplicationService
    {
        Task<long> Create(InventoryCreate input);
        Task<PagedResultDto<InventoryListDto>> GetAll(InventorySearch input);
        Task<InventoryListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(InventoryListDto input);
        Task<long> UpdateExport(InventoryListDto input);
        Task<long> Delete(long id);


        Task<PagedResultDto<InventoryListDto>> GetAllItemStock(InventorySearch input);

        //code ha
        Task<InventoryListDto> GetInventoryByCode(string itemCode);

        Task<PagedResultDto<InventoryListDto>> GetAllItem(InventorySearch input);
        Task<PagedResultDto<InventoryListDto>> GetAllInStock(InventoryInputSearch input);
        Task<long> DeleteByHa(InventorySearch input);
        Task<InventoryListDto> GetViewItemInWare(InventorySearch input);
        Task<long> UpdateImport(InventoryCreate input);

    }
}
