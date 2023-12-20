using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseCards.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseCards.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseCards
{
  public interface IWarehouseCardAppService : IApplicationService
    {
        Task<long> Create(WarehouseCardCreate input);
        Task<PagedResultDto<WarehouseCardListDto>> GetAll(WarehouseCardSearch input);
        Task<WarehouseCardListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(WarehouseCardCreate input);
        Task<long> Delete(long id);
       

    }
}
