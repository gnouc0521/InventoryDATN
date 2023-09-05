using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Categories;
using bbk.netcore.mdl.PersonalProfile.Application.CivilServants;
using bbk.netcore.mdl.PersonalProfile.Application.Documents;
using bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles;
using bbk.netcore.mdl.PersonalProfile.Application.Reports.Exporting;
using bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses;
using bbk.netcore.mdl.PersonalProfile.Application.Statistics.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.DocumentEnum;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Statistics
{
    [AbpAuthorize]
    public class StatisticAppService : ApplicationService, IStatisticAppService
    {
        private readonly IPersonalProfileAppService _personalProfileAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly ICivilServantAppService _civilServantAppService;
        private readonly IWorkingProcessAppService _workingProcessAppService;
        private readonly ISalaryProcessAppService _salaryProcessAppService;
        private readonly IAbpSession _session;
        private readonly IOrganizationUnitStaffAppService _organizationUnitStaffAppService;
        private readonly IReportExcelExporter _reportExcelExporter;

        public StatisticAppService(
            IPersonalProfileAppService personalProfileAppService,
            ICategoryAppService categoryAppService,
            ICivilServantAppService civilServantAppService,
            IWorkingProcessAppService workingProcessAppService,
            ISalaryProcessAppService salaryProcessAppService,
            IAbpSession session,
            IOrganizationUnitStaffAppService organizationUnitStaffAppService,
            IReportExcelExporter reportExcelExporter)
        {
            _personalProfileAppService = personalProfileAppService;
            _categoryAppService = categoryAppService;
            _civilServantAppService = civilServantAppService;
            _workingProcessAppService = workingProcessAppService;
            _salaryProcessAppService = salaryProcessAppService;
            _session = session;
            _organizationUnitStaffAppService = organizationUnitStaffAppService;
            _reportExcelExporter = reportExcelExporter;
        }


        public async Task<PagedResultDto<StatisticDto>> GetAll(GetStatisticInput input)
        {
            try
            {
                var list = await GetAllList(input);
                var result = list.Skip(input.SkipCount.Value).Take(input.MaxResultCount.Value);
                return new PagedResultDto<StatisticDto>(list.Count(), result.ToList());
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        private async Task<List<StatisticDto>> GetAllList(GetStatisticInput input)
        {
            try
            {
                var userId = _session.UserId;
                var list = (from person in _personalProfileAppService.GetAllList().Result.ToList()
                       .WhereIf(!string.IsNullOrEmpty(input.Ethnic) && !string.IsNullOrWhiteSpace(input.Ethnic), u => u.Ethnic.ToLower().Contains(input.Ethnic.Trim().ToLower().ToString()))
                       .WhereIf(input.Gender.HasValue, u => u.Gender == input.Gender)
                       .WhereIf(input.FromRecruitmentDate.HasValue, u => u.RecruitmentDate >= input.FromRecruitmentDate)
                       .WhereIf(input.ToRecruitmentDate.HasValue, u => u.RecruitmentDate <= input.ToRecruitmentDate)
                       .WhereIf(input.AcademicTitle.Where(u => u != null && u != 0).Count() > 0, u => input.AcademicTitle.Contains(u.AcademicTitle))
                       .WhereIf(input.IsPartyMember.HasValue, u => (u.OfficialDateOfJoiningInVNCommunistParty != null && input.IsPartyMember == true) ||
                       (u.OfficialDateOfJoiningInVNCommunistParty == null && input.IsPartyMember == false))
                       .WhereIf(input.PoliticsTheoReticalLevel.Where(u => u != null && u != 0).Count() > 0, u => input.PoliticsTheoReticalLevel.Contains(u.PoliticsTheoReticalLevel)).ToList()
                            join category in _categoryAppService.GetListPosition().Result on person.CurrentPosition equals category.Id
                            join academicLevel in _categoryAppService.GetListByType(CategoryType.Diploma).Result.Items on person.HighestAcademicLevel equals academicLevel.Id
                            join salaryLevel in _civilServantAppService.GetAllSalaryLevel().Result on person.PayRate equals salaryLevel.Id
                            join organizationUnitStaff in _organizationUnitStaffAppService.GetAll().Result on person.Id equals organizationUnitStaff.StaffId
                            join organizationUnit in _organizationUnitStaffAppService.GetListOrgByUserId((long)userId).Result on organizationUnitStaff.OrganizationUnitId equals organizationUnit.Id
                            join civilServant in _civilServantAppService.GetAllCivilServant().Result on person.CivilServantSector equals civilServant.Id
                            join salaryProcess in _salaryProcessAppService.GetAllList().Result on person.Id equals salaryProcess.PersonId into sp
                            from salaryP in sp.DefaultIfEmpty()
                            select new
                            {
                                Id = person.Id,
                                OrgId = organizationUnitStaff.OrganizationUnitId,
                                FullName = person.FullName,
                                RecruitmentDate = person.RecruitmentDate,
                                DateOfBirth = person.DateOfBirth,
                                HighestAcademicLevel = academicLevel.Title,
                                Specialized = person.Specialized,
                                Age = GetAge.GetRealAge(person.DateOfBirth),
                                NativePlace = person.VillageOfNativePlace + ", " + person.DistrictOfNativePlace + ", " + person.ProvinceOfNativePlace,
                                PositionId = category.Id,
                                Position = category.Title,
                                CivilServant = civilServant.Name,
                                CivilServantCode = civilServant.Code,
                                CoefficientsSalary = salaryLevel.CoefficientsSalary,
                                PositionAllowance = person.PositionAllowance,
                                OtherAllowance = person.OtherAllowance,
                                TypeServant = (int)salaryLevel.Group <= 10 ? ((int)salaryLevel.Group >= 0 ? StatusEnum.CategoryType.CivilServant : 0) : StatusEnum.CategoryType.PublicServant,
                                SalaryRaiseTime = salaryP?.SalaryIncreaseTime ?? null,
                                LeadershipPositionAllowance = !string.IsNullOrEmpty(salaryP?.LeadershipPositionAllowance) ? salaryP?.LeadershipPositionAllowance : null,
                                ToxicAllowance = !string.IsNullOrEmpty(salaryP?.ToxicAllowance) ? salaryP?.ToxicAllowance : null,
                                AreaAllowance = !string.IsNullOrEmpty(salaryP?.AreaAllowance) ? salaryP?.AreaAllowance : null,
                                ResponsibilityAllowance = !string.IsNullOrEmpty(salaryP?.ResponsibilityAllowance) ? salaryP?.ResponsibilityAllowance : null,
                                MobileAllowance = !string.IsNullOrEmpty(salaryP?.MobileAllowance) ? salaryP?.MobileAllowance : null,
                            }).WhereIf(input.TypeServant.HasValue, u => u.TypeServant == (StatusEnum.CategoryType)input.TypeServant)
                        .WhereIf(input.Position.Where(u => u != null && u != 0).Count() > 0, u => input.Position.Contains(u.PositionId))
                        .WhereIf(input.OrgId.HasValue, u => u.OrgId == input.OrgId)
                        .GroupBy(u => new
                        {
                            Id = u.Id
                        })
                       .Select(y => y.First())
                        .WhereIf(input.SalaryRaiseTime.HasValue, u => u.SalaryRaiseTime == input.SalaryRaiseTime)
                        .WhereIf(input.Allowance.Where(u => u != null).Count() > 0, u => (u.LeadershipPositionAllowance != null && input.Allowance.Contains(AllowanceType.LeadershipPosition))
                        || (u.ToxicAllowance != null && input.Allowance.Contains(AllowanceType.Toxic))
                        || (u.AreaAllowance != null && input.Allowance.Contains(AllowanceType.Area))
                        || (u.ResponsibilityAllowance != null && input.Allowance.Contains(AllowanceType.Responsibility))
                        || (u.MobileAllowance != null && input.Allowance.Contains(AllowanceType.Mobile)))
                        .WhereIf(input.MaxAge.HasValue && input.MinAge.HasValue, u => u.Age >= input.MinAge && u.Age <= input.MaxAge)
                       .Select((u, i) => new StatisticDto
                       {
                           STT = i + 1,
                           Id = u.Id,
                           FullName = u.FullName.Trim(),
                           DateOfBirth = u.DateOfBirth,
                           Age = u.Age,
                           Specialized = u.Specialized,
                           TimeIncreaseSalary = u.SalaryRaiseTime,
                           HighestAcademicLevel = u.HighestAcademicLevel,
                           Position = u.Position,
                           RecruitmentDate = u.RecruitmentDate,
                           CivilServant = u.CivilServant,
                           CoefficientsSalary = u.CoefficientsSalary,
                           NativePlace = u.NativePlace,
                           CivilServantCode = u.CivilServantCode
                       }).ToList();
                return await Task.FromResult(list);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> ExportStatistic(GetStatisticInput input)
        {
            try
            {
                var result = await GetAllList(input);
                return await _reportExcelExporter.ExportStatisticToFile(result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
