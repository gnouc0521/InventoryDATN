using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ScheduleWorks.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ScheduleWorks
{
    public interface IScheduleWorkAppService : IApplicationService
    {
        Task<PagedResultDto<ScheduleWorkListDto>> GetAll(GetScheduleWorkInput input);
        Task<ScheduleWorkListDto> GetAsync(EntityDto itemId);
        Task<int> Create(ScheduleWorkCreateDto input);
        Task<int> Update(ScheduleWorkEditDto input);
        Task<int> Delete(int id);
    }
}
