using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.CommunistPartyProcesses.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.CommunistPartyProcesses
{
    public interface ICommunistPartyProcessAppService : IApplicationService
    {
        Task<int> Create(CommunistPartyProcessDto dto);

        Task<int> Update(CommunistPartyProcessDto dto);

        Task<List<GetCommunistPartyProcessDto>> GetAll(GetAllFilter filter);

        Task<CommunistPartyProcessDto> GetById(int id);

        Task DeleteById(DeleteCommunistPartyProcessDto deleteCommunistPartyProcessDto);
    }
}
