using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Itemses
{
  public interface IItemsService : IApplicationService
    {
        Task<long> Create(ItemsListDto input);
        Task<PagedResultDto<ItemsListDto>> GetAll(ItemsSearch input);
        Task<PagedResultDto<ItemsListDto>> GetAllItems(ItemsSearch input);
        Task<ItemsListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(ItemsListDto input);
        Task<long> Delete(long id);
        Task<List<ItemsListDto>> GetItemList();

        Task<List<ItemsListDto>> GetItemImportList();

        //code ha
        Task<ItemsListDto> GetItemByCode(string itemCode);
    }
}
