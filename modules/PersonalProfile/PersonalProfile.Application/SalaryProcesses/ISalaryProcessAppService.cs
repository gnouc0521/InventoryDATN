using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses
{
   public interface ISalaryProcessAppService : IApplicationService
    {
        Task<List<SalaryProcessDto>> GetAll(int staffId);

        Task<SalaryProcessDto> GetById(int id);

        Task Update(SalaryProcessDto salaryProcessDto);

        Task<List<SalaryProcessDto>> GetAllList();

        List<int> GetAllStaffIdByIssuedTime(DateTime fromDate, DateTime toDate);

        List<int> GetAllListStaffId();
    }
}
