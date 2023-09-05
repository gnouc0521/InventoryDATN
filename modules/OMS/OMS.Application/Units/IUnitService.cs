using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ScheduleWorks.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Units
{
    public interface IUnitService : IApplicationService
    {
        Task<PagedResultDto<UnitListDto>> GetAll(UnitSearch input);
        Task<UnitListDto> GetAsync(EntityDto itemId);
        Task<int> Create(UnitListDto input);
        Task<int> Update(UnitListDto input);
        Task<int> Delete(int id);
        Task<List<UnitListDto>> GetUnitList();
        Task<List<UnitListDto>> GetUnitListDtos();
        Task<UnitListDto> GetUnitByText(string NameText);
    }
}
