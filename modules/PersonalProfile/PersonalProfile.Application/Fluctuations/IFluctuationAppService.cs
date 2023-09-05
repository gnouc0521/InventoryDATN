using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.Fluctuations.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Fluctuations
{
    public interface IFluctuationAppService : IApplicationService
    {
        Task<List<FluctuationDto>> GetContractExpiration ();

        Task<List<FluctuationDto>> GetReappointed();

        Task<List<FluctuationDto>> GetSalaryIncrease();

        Task<List<FluctuationDto>> GetRetirement();
    }
}
