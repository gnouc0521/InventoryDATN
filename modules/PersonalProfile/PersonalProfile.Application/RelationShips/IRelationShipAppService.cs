using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.RelationShips.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.RelationShips
{
    public interface IRelationShipAppService : IApplicationService
    {
        Task<int> Create(RelationShipDto relationShipDto);

        Task Update(RelationShipDto relationShipDto);

        Task<List<RelationShipDto>> GetAll(GetAllFilter filter);

        Task<RelationShipDto> GetById(int id);

        Task DeleteById(DeleteRelationShipDto deleteRelationShipDto);
    }
}
