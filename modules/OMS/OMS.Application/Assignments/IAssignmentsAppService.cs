using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Assignments.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Assignments
{
    public interface IAssignmentsAppService : IApplicationService
    {
        Task<PagedResultDto<AssignmentsListDto>> GetAll();
        Task<int> Create(AssignmentsListDto input);
        Task<int> Delete(int id);
        Task<int> DeletebyItem(int idtem);
        Task<PagedResultDto<ItemsListDto>> GetAllItemByUserId(long userId);
        Task<PagedResultDto<GetAssignmentsListDto>> GetAllByUser(int Id);
        Task<long> Update(AssignmentsListDto input);
        }
}
