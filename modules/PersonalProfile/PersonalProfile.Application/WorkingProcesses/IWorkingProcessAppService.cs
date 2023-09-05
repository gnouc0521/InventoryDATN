using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses
{
    public interface IWorkingProcessAppService : IApplicationService
    {
        Task Create(WorkingProcessDto workingProcessDto);

        Task Update(WorkingProcessDto workingProcessDto);

        Task<List<GetWorkingProcessDto>> GetAll(GetAllWorkingProcessFilter filter);

        Task<WorkingProcessDto> GetById(int id);

        Task DeleteById(DeleteWorkingProcessDto deleteWorkingProcessDto);

        Task<List<GetWorkingProcessDto>> GetAllListDetail();

        Task<List<WorkingProcessDto>> GetAllList();

        List<int> GetAllListStaffId();
    }
}
