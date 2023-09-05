using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseTypes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Ruleses
{
    public interface IRulesService : IApplicationService
    {
        Task<PagedResultDto<RulesListDto>> GetAll(RulesSearch input);
        Task<RulesListDto> GetAsync(EntityDto itemId);
        Task<int> Create(RulesListDto input);
        Task<int> Update(RulesListDto input);
        Task<int> Delete(int id);

        Task<List<RulesListDto>> GetAllCategory();
        Task<List<RulesListDto>> GetAllGroup();
        Task<List<RulesListDto>> GetAllKind();
    }
}
