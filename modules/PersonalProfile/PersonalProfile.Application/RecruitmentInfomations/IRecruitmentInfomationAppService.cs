
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.RecruitmentInfomations.Dtos;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.RecruitmentInfomations
{
   public interface IRecruitmentInfomationAppService: IApplicationService
   {
        Task<ListResultDto<RecruitmentInfomationDto>> GetAll(int staffId);
        Task<ListResultDto<RecruitmentInfomationDto>> GetAllList();
        Task Create(RecruitmentInfomationDto input);
        Task<RecruitmentInfomationDto> GetById(int id);
        Task Update(RecruitmentInfomationDto recruitmentInfomationDto);
    }

    
}
