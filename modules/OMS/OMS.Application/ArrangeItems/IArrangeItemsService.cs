using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ArrangeItems.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ArrangeItems
{
  public interface IArrangeItemsService : IApplicationService
    {
        Task<PagedResultDto<ArrangeItemsListDto>> GetAll();
    }
}
