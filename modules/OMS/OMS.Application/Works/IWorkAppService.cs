using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Works
{
    public interface IWorkAppService: IApplicationService
    {
        Task<PagedResultDto<WorkListDto>> GetAll(GetWorkInput input);
        Task<WorkEditDto> GetAsync(EntityDto itemId);
        Task<long> Create(WorkCreateDto input);
        Task<long?> Update(WorkEditDto input);
        Task<WorkListDto> GetById(long id);
        bool CheckDayOff(DateTime startDate, DateTime endDate);

        bool CheckDayOffStart(DateTime startDate);
        //object GetAllByCoWork();
        Task<long> Changstatus(WorkListDto input);
        Task<PagedResultDto<WorkListDto>> GetAllByCoWork();

        //Hà thêm
        Task<PagedResultDto<UsersListDto>> GetAlUsers();
        Task<PagedResultDto<WorkListDto>> GetAllByStatus(GetWorkListNumOfType StatusId);
        Task<PagedResultDto<WorkListDto>> GetAllByTime(GetWorkListNumInput input);
        Task<PagedResultDto<WorkListDto>> GetAllByTimeInBox();
        Task<PagedResultDto<WorkListDto>> GetAllInDashBoard(GetWorkListNumInput input);

    }
}
