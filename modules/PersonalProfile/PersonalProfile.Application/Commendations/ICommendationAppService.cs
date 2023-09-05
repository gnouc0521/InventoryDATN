using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.Commendations.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.Commendations
{
    public interface ICommendationAppService : IApplicationService
    {
        Task<int> Create(CommendationDto commendationDto);

        Task Update(CommendationDto commendationDto);

        Task<List<GetCommendationDto>> GetAll(GetAllFilter filter);

        Task<CommendationDto> GetById(int id);

        Task DeleteById(DeleteCommendationDto deleteCommendationDto);

        Task<List<GetCommendationDto>> GetAllList();
    }
}
