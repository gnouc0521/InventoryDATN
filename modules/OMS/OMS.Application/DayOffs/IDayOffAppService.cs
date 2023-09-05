using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.DayOffs.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.DayOffs
{
    public interface IDayOffAppService : IApplicationService
    {
        Task<PagedResultDto<DayOffListDto>> GetAll(GetDayOffInput input);
        Task<DayOffListDto> GetAsync(EntityDto itemId);
        Task<int> Create(DayOffCreateDto input);
        Task<int> Update(DayOffEditDto input);
        Task<int> Delete(int id);
        Task<List<string>> GetAllDayOffsIf();
        Task<List<string>> GetAllDayOffsIfById(int id);
        object GetAllDate(DateTime startDate);
    }
}
