using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.GoAbroads.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.GoAbroads
{
    public interface IGoAbroadAppService : IApplicationService
    {
        Task<int> Create(GoAbroadDto goAbroadDto);

        Task Update(GoAbroadDto goAbroadDto);

        Task<List<GetGoAbroadDto>> GetAll(GetAllFilter filter);

        Task<GoAbroadDto> GetById(int id);

        Task DeleteById(DeleteGoAbroadDto deleteGoAbroadDto);
    }
}
