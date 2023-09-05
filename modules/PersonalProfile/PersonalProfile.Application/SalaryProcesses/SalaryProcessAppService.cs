using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using bbk.netcore.mdl.PersonalProfile.Application.Categories;
using bbk.netcore.mdl.PersonalProfile.Application.Documents;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles;
using bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs;
using bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses.Dtos;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using bbk.netcore.Authorization;

namespace bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses
{
    public class SalaryProcessAppService :  ApplicationService, ISalaryProcessAppService
    {
        private readonly IRepository<SalaryProcess> _salaryProcessreponsitory;
        private readonly IDocumentAppService _documentAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IPersonalProfileAppService _personalProfileAppService;


        public SalaryProcessAppService(
            IRepository<SalaryProcess> salaryProcessreponsitory,
            IDocumentAppService documentAppService,
            ICategoryAppService categoryAppService,
            IPersonalProfileAppService personalProfileAppService)
        {
            _personalProfileAppService = personalProfileAppService;
            _salaryProcessreponsitory = salaryProcessreponsitory;
            _documentAppService = documentAppService;
            _categoryAppService = categoryAppService;
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<SalaryProcessDto>> GetAllList()
        {
            var query = await _salaryProcessreponsitory.GetAll()
                .OrderByDescending(u => u.SalaryIncreaseTime)
                .ToListAsync();
            return ObjectMapper.Map<List<SalaryProcessDto>>(query);

        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<SalaryProcessDto>> GetAll(int staffId)
         {
            var SalaryProcess = await _salaryProcessreponsitory
                .GetAll()
                .Where(u => u.PersonId == staffId)
                .OrderByDescending(t => t.SalaryIncreaseTime)
                .ToListAsync();
            return new List<SalaryProcessDto>(
                ObjectMapper.Map<List<SalaryProcessDto>>(SalaryProcess)
            );

        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Create(SalaryProcessDto input)
        {
            try
            {
                var payrates = input.PayRate.Split('/')[1];
                switch (payrates)
                {

                    case "6":
                        {
                            input.NextSalaryIncreaseTime = input.SalaryIncreaseTime.AddMonths(60);
                            break;
                        }
                    case "9":
                        {
                            input.NextSalaryIncreaseTime = input.SalaryIncreaseTime.AddMonths(36);
                            break;
                        }
                    case "10":
                        {
                            input.NextSalaryIncreaseTime = input.SalaryIncreaseTime.AddMonths(36);
                            break;
                        }
                    case "12":
                        {
                            input.NextSalaryIncreaseTime = input.SalaryIncreaseTime.AddMonths(24);
                            break;
                        }
                    default:
                        input.NextSalaryIncreaseTime = input.SalaryIncreaseTime;
                        break;

                }
                var entity = ObjectMapper.Map<SalaryProcess>(input);
                    int id = await _salaryProcessreponsitory.InsertAndGetIdAsync(entity);               
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(int salaryProcessId)
        {
            try
            {
                var salaryProcess = _salaryProcessreponsitory.Get(salaryProcessId);
                if(salaryProcess == null) {
                    throw new Exception("ID không tồn tại!");
                }
                await _salaryProcessreponsitory.DeleteAsync(salaryProcess);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<SalaryProcessDto> GetById(int id)
        {
            try
            {
                var entity = await _salaryProcessreponsitory.FirstOrDefaultAsync(id);
                if (entity == null)
                {
                    throw new Exception("Chi tiết quá trình lương không đúng!");
                }
                SalaryProcessDto salaryProcessDto = ObjectMapper.Map<SalaryProcessDto>(entity);
                return salaryProcessDto;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Update(SalaryProcessDto input)
        {
            try
            {
                int id = input.Id;
                var checkSalaryProcess = await _salaryProcessreponsitory.FirstOrDefaultAsync(x => x.Id == id);
                if (checkSalaryProcess == null)
                {
                    throw new Exception("Chi tiết quá trình lương không đúng!");
                }
                var payrates = input.PayRate.Split('/')[1];
                switch (payrates)
                {

                    case "6":
                        {
                            input.NextSalaryIncreaseTime = input.SalaryIncreaseTime.AddMonths(60);
                            break;
                        }
                    case "9":
                        {
                            input.NextSalaryIncreaseTime = input.SalaryIncreaseTime.AddMonths(36);
                            break;
                        }
                    case "10":
                        {
                            input.NextSalaryIncreaseTime = input.SalaryIncreaseTime.AddMonths(36);
                            break;
                        }
                    case "12":
                        {
                            input.NextSalaryIncreaseTime = input.SalaryIncreaseTime.AddMonths(24);
                            break;
                        }
                    default:
                        input.NextSalaryIncreaseTime = input.SalaryIncreaseTime;
                        break;

                }
                ObjectMapper.Map(input, checkSalaryProcess);                             
            
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public List<int> GetAllStaffIdByIssuedTime(DateTime fromDate, DateTime toDate)
        {
            var lst = (from salary in _salaryProcessreponsitory.GetAllList(x => x.IssuedTime >= fromDate && x.IssuedTime <= toDate).ToList()
                       join person in _personalProfileAppService.GetAllList().Result on salary.PersonId equals person.Id
                       select salary.PersonId).Distinct().ToList();

            return lst;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public List<int> GetAllListStaffId()
        {
            var list = _salaryProcessreponsitory.GetAll().Select(x => x.PersonId).Distinct().ToList();
            return list;
        }
      

  }
}

