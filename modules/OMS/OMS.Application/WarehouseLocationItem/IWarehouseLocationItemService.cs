using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseLocationItem.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseLocationItem
{
    public interface IWarehouseLocationItemService : IApplicationService
    {
        Task<long> Create(WarehouseLocationItemsCreateDto input);
        Task<int> TotalInLocation(int idlocation);
        Task<PagedResultDto<ItemsListDto>> GetAll(WarehouseLocationItemsInput input);
        Task<PagedResultDto<WarehouseLocationItemsDto>> GetAllListItem(WarehouseLocationItemsInput input);
        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Các phiếu nhập kho chư sắp xếp </returns>
        Task<PagedResultDto<WarehouseLocationItemsDto>> GetAllItems(WarehouseLocationItemsInput input);
        Task<PagedResultDto<WarehouseLocationItemsDto>> GetAllItem(WarehouseLocationItemsInput input);
        Task<PagedResultDto<WarehouseLocationItemsDto>> GetLocationItems(WarehouseLocationItemsInput input);
        Task<long> Update(WarehouseLocationItemsDto input);
        Task<WarehouseLocationItemsDto> GetAsync(EntityDto itemId);

        //code ha
        Task<PagedResultDto<WarehouseLocationItemsDto>> GetByIdBlock(WarehouseLocationItemsInput input);
        Task<long> Delete(WarehouseLocationItemsInput input);
        Task<long> CountInLocation(WarehouseLocationItemsInput input);
        Task<long> CountInLocationFree(WarehouseLocationItemsInput input);
        Task<PagedResultDto<WarehouseLocationItemsDto>> GetItemSameInWare(WarehouseLocationItemsInput input);

        Task<long> UpdateItemQuantity(WarehouseLocationItemsDto input);

        Task<PagedResultDto<WarehouseLocationItemsDto>> GetByInImport(WarehouseLocationItemsInput input);
        Task<long> UpdateIpoSubmit(WarehouseLocationItemsDto input);

    }
}
