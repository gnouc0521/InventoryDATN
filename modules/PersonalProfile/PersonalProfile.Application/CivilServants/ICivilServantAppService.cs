using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.CivilServants.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.CivilServants
{
    public interface ICivilServantAppService : IApplicationService
    {
        Task<SalaryLevelDto> GetSalaryLevel(int id);

        Task<List<SalaryLevelDto>> GetAllSalaryLevel();

        Task<List<CivilServantDto>> GetAllCivilServant();
    }
}
