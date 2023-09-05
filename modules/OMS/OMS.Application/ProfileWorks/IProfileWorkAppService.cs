using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ProfileWorks
{
    public interface IProfileWorkAppService : IApplicationService
    {
        Task<PagedResultDto<ProfileWorkListDto>> GetAll(GetProfileWorkInput input);
        Task<PagedResultDto<ProfileWorkListDto>> GetAllList();
        Task<PagedResultDto<ProfileWorkListDto>> GetAsync(EntityDto itemId);
        //Task<ProfileWorkEditDto> GetAsync(EntityDto itemId);
        Task<int> Create(ProfileWorkCreateDto input);
        Task<long> Update(ProfileWorkEditDto input);
        Task<int> Delete(int id);
        Task<PagedResultDto<ProfileWorkListDto>> GetAllListItem(int id);

        Task<PagedResultDto<ProfileWorkListDto>> GetAllListParent();

        //Code Hà
        Task<PagedResultDto<WorkListDto>> GetAllWorkByProfileWorkId(GetProfileWorkInput input);
        Task<PagedResultDto<WorkListDto>> GetAllFileByProfileWorkId(EntityDto itemId);
        Task<PagedResultDto<WorkListDto>> GetAllWorkByProfileWorkIdInChart(GetProfileWorkInput input);
    }
}
