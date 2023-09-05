using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.WorkGroups.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WorkGroups
{
    public interface IWorkGroupAppService : IApplicationService
    {
        Task<PagedResultDto<WorkGroupListDto>> GetAll(GetWorkGroupInput input);
        Task<PagedResultDto<WorkGroupListDto>> GetAllList();
        Task<PagedResultDto<WorkGroupListDto>> GetAllListParent();

        Task<PagedResultDto<WorkGroupListDto>> GetAllListItem(int id);
        Task<PagedResultDto<WorkGroupListDto>> GetAsync(EntityDto itemId);
        Task<int> Create(WordGroupCreateDto input);
        Task<int> Update(WorkGroupEditDto input);
        Task<int> Delete(int id);
    }
}
