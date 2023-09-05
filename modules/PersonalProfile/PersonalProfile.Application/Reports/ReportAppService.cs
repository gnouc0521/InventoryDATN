using Abp.Application.Services;
using Abp.Authorization;
using Abp.Timing;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.AssessedByYears;
using bbk.netcore.mdl.PersonalProfile.Application.AssessedByYears.Dtos;
using bbk.netcore.mdl.PersonalProfile.Application.Categories;
using bbk.netcore.mdl.PersonalProfile.Application.CivilServants;
using bbk.netcore.mdl.PersonalProfile.Application.Commendations;
using bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.PropertyDeclarations;
using bbk.netcore.mdl.PersonalProfile.Application.PropertyDeclarations.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.RecruitmentInfomations;
using bbk.netcore.mdl.PersonalProfile.Application.RelationShips;
using bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Reports.Exporting;
using bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses;
using bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses.Dtos;
using bbk.netcore.mdl.PersonalProfile.Application.TrainningInfos;
using bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses;
using bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports
{
    [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Reports)]
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly ISalaryProcessAppService _salaryProcessAppService;
        private readonly IPersonalProfileAppService _personalProfileAppService;
        private readonly IWorkingProcessAppService _workingProcessAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IReportExcelExporter _reportExcelExporter;
        private readonly IOrganizationUnitStaffAppService _organizationUnitStaffAppService;
        private readonly ICivilServantAppService _civilServantAppService;
        private readonly ITrainningInfoAppService _trainningInfoAppService;
        private readonly IPropertyDeclarationAppService _propertyDeclarationAppService;
        private readonly IAssessedByYearAppService _assessedByYearAppService;
        private readonly ICommendationAppService _commendationAppService;
        private readonly IRecruitmentInfomationAppService _recruitmentInfomationAppService;
        private readonly IOrganizationUnitAppService _organizationUnitAppService;
        private readonly IRelationShipAppService _relationShipAppService;

        public ReportAppService(ISalaryProcessAppService salaryProcessAppService,
            IPersonalProfileAppService personalProfileAppService,
            IWorkingProcessAppService workingProcessAppService,
            ICategoryAppService categoryAppService,
            IReportExcelExporter reportExcelExporter,
            IOrganizationUnitStaffAppService organizationUnitStaffAppService,
            ICivilServantAppService civilServantAppService,
            IPropertyDeclarationAppService propertyDeclarationAppService,
            ICommendationAppService commendationAppService,
            IRecruitmentInfomationAppService recruitmentInfomationAppService,
            IAssessedByYearAppService assessedByYearAppService,
            ITrainningInfoAppService trainningInfoAppService,
            IOrganizationUnitAppService organizationUnitAppService,
            IRelationShipAppService relationShipAppService
            )
        {
            _categoryAppService = categoryAppService;
            _salaryProcessAppService = salaryProcessAppService;
            _personalProfileAppService = personalProfileAppService;
            _workingProcessAppService = workingProcessAppService;
            _reportExcelExporter = reportExcelExporter;
            _organizationUnitStaffAppService = organizationUnitStaffAppService;
            _civilServantAppService = civilServantAppService;
            _trainningInfoAppService = trainningInfoAppService;
            _propertyDeclarationAppService = propertyDeclarationAppService;
            _assessedByYearAppService = assessedByYearAppService;
            _commendationAppService = commendationAppService;
            _recruitmentInfomationAppService = recruitmentInfomationAppService;
            _organizationUnitAppService = organizationUnitAppService;
            _relationShipAppService = relationShipAppService;
        }
        
        public async Task<FileDto> Report(ReportFilterDto reportFilterDto)
        {
            try
            {
                switch (reportFilterDto.ReportEnum)
                {
                    case ReportEnum.ReportTypeEnum.BM01a:
                        return await GetBM01aListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM02:
                        return await GetBM02ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM03:
                        return await GetBM03ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM04:
                        return await GetBM04ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM05:
                        return await GetBM05ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM07:
                        return await GetBM07ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM08:
                        return await GetBM08ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM09:
                        return await GetBM09ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM10:
                        return await GetBM10ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM12:
                        return await GetBM12ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM13:
                        return await GetBM13ListDto(reportFilterDto);
                    case ReportEnum.ReportTypeEnum.BM15:
                        return await GetBM15ListDto(reportFilterDto);
                    default:
                        throw new UserFriendlyException("Báo cáo lỗi!");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM01aListDto(ReportFilterDto input)
        {
            try
            {
                if (!input.OrgId.HasValue)
                {
                    throw new UserFriendlyException("Vui lòng chọn đơn vị");
                }

                List<BM01aDto> result = new List<BM01aDto>();

                var orgs = await _organizationUnitAppService.GetOrganizationUnits();

                var org = orgs.Items.Where(u => u.Id == input.OrgId).FirstOrDefault();

                var staffs = (from person in _personalProfileAppService.GetAllList().Result
                              join orgStaff in _organizationUnitStaffAppService.GetAll().Result.Where(u => u.OrganizationUnitId == input.OrgId)
                              on person.Id equals orgStaff.StaffId
                              select new PersonalProfileDto
                              {
                                  Id = person.Id,
                                  FullName = person.FullName
                              }).ToList();
                int stt = 1;
                foreach(var staff in staffs)
                {
                    var workingProcess = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter { PersonId = staff.Id });

                    var wps = workingProcess.Where(u => u.TypeOfChange.Trim().ToLower() == ("Ký hợp đồng lao động").Trim().ToLower()
                        && u.FromDate.Year <= DateTime.Today.Year && (u.ToDate.HasValue && u.ToDate.Value.Year >= DateTime.Today.Year))
                        .OrderByDescending(u => u.FromDate).FirstOrDefault();

                    if(wps != null)
                    {
                        var bm01a = new BM01aDto
                        {
                            STT = stt,
                            FullName = staff.FullName,
                            DateNumber = wps.DecisionNumber + "; " + wps.IssuedDate?.ToShortDateString(),
                            OrganizationUnit = wps.DepartmentName + ", " + org.DisplayName,
                            Work = wps.WorkingTitle,
                            ConstractDuration = wps.ToDate,
                            TitleOfConstractSigner = wps.DecisionMaker,
                            TotalContract = workingProcess.Where(u => u.TypeOfChange.Trim().ToLower() == ("Ký hợp đồng lao động").Trim().ToLower()).Count()
                        };
                        result.Add(bm01a);
                        stt++;
                    }
                }

                return await _reportExcelExporter.ExportBM01aToFile(result, DateTime.Today.Year, org.DisplayName);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM02ListDto(ReportFilterDto reportFilterDto)
        {
            if (!reportFilterDto.OrgId.HasValue)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn vị");
            }
            if (!reportFilterDto.FromDate.HasValue)
            {
                reportFilterDto.FromDate = DateTime.Now;
            }
            if (!reportFilterDto.ToDate.HasValue)
            {
                reportFilterDto.ToDate = DateTime.Now;
            }
            if(reportFilterDto.FromDate > reportFilterDto.ToDate)
            {
                throw new UserFriendlyException("Ngày kết thúc đang sớm hơn ngày bắt đầu");
            }
            var fromDate = reportFilterDto.FromDate.Value;
            var toDate = reportFilterDto.ToDate.Value;
            var lstPersonId = _salaryProcessAppService.GetAllStaffIdByIssuedTime(reportFilterDto.FromDate.Value, reportFilterDto.ToDate.Value);

            lstPersonId = (from l in _organizationUnitStaffAppService.GetAll().Result
                     join
                    p in lstPersonId on l.StaffId equals p
                    where reportFilterDto.OrgId.HasValue && l.OrganizationUnitId == reportFilterDto.OrgId.Value
                    select l.StaffId).Distinct().ToList();
            List<Bm02Dto> bm02Dtos = new List<Bm02Dto>();

            // Tìm ra chi tiết cho từng nhân viên
            foreach (var personId in lstPersonId)
            {
                CoefficientsSalaryDetail currentCoefficientsSalaryDetail = new CoefficientsSalaryDetail();
                CoefficientsSalaryDetail oldCoefficientsSalaryDetail = new CoefficientsSalaryDetail();
                Bm02Dto bm02Dto = new Bm02Dto();

                var lstsalaryProcess = await _salaryProcessAppService.GetAll(personId);

                var staffDetail = await _personalProfileAppService.Get(personId);

                var currentlstsalaryProcess = lstsalaryProcess.Where(x => x.IssuedTime >= fromDate && x.IssuedTime <= toDate)
                    .OrderByDescending(x => x.IssuedTime).ToList();

                // Nếu mà tìm được mức lương trong khoảng đã cho thì mới tìm chi tiết
                if (currentlstsalaryProcess.Count > 0)
                {
                    bm02Dto.FullName = staffDetail.FullName;
                    if (staffDetail.Gender == Gender.Male) { bm02Dto.MaleDateTime = staffDetail.DateOfBirth; }
                    else { bm02Dto.FeMaleDateTime = staffDetail.DateOfBirth; }

                    currentCoefficientsSalaryDetail.Glone = currentlstsalaryProcess.First().Glone;
                    currentCoefficientsSalaryDetail.PayRate = currentlstsalaryProcess.First().PayRate;
                    currentCoefficientsSalaryDetail.CoefficientsSalary = currentlstsalaryProcess.First().CoefficientsSalary.ToString();
                    currentCoefficientsSalaryDetail.FromDate = currentlstsalaryProcess.First().SalaryIncreaseTime;
                    currentCoefficientsSalaryDetail.PositionAllowance = currentlstsalaryProcess.First()?.LeadershipPositionAllowance ?? "";
                    currentCoefficientsSalaryDetail.OtherAllowance = GetOtherAllowanceBM02(currentlstsalaryProcess.First());
                    // Tìm quá trình công tác theo theo thời gian của quyết định nâng lương
                    var workingProcesses = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = personId });
                    workingProcesses = workingProcesses.Where(x =>
                        x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                        && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                        && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower())
                        && x.FromDate >= currentlstsalaryProcess.First().IssuedTime // kiểm tra ngày bắt đầu công tác phải sau ngày quyết định nâng lương
                        ).OrderByDescending(x => x.FromDate).ToList();
                    if (workingProcesses.Count > 0)
                    {
                        bm02Dto.Year = workingProcesses.Last().FromDate;
                        currentCoefficientsSalaryDetail.OrganName = workingProcesses.First().OrganName;
                        currentCoefficientsSalaryDetail.JobPosition = workingProcesses.First().JobPosition;
                        currentCoefficientsSalaryDetail.WorkingTitle = workingProcesses.First().WorkingTitle;
                    }
                    else bm02Dto.Year = null;

                    bm02Dto.currentCoefficientsSalaryDetail = currentCoefficientsSalaryDetail;

                    // Tìm chi tiết quá trình công tác trước đó
                    if (currentlstsalaryProcess.Count > 1)
                    {
                        oldCoefficientsSalaryDetail.Glone = currentlstsalaryProcess[1].Glone;
                        oldCoefficientsSalaryDetail.PayRate = currentlstsalaryProcess[1].PayRate;
                        oldCoefficientsSalaryDetail.CoefficientsSalary = currentlstsalaryProcess[1].CoefficientsSalary.ToString();
                        oldCoefficientsSalaryDetail.FromDate = currentlstsalaryProcess[1].SalaryIncreaseTime;
                        oldCoefficientsSalaryDetail.PositionAllowance = currentlstsalaryProcess[1]?.LeadershipPositionAllowance ?? "";
                        oldCoefficientsSalaryDetail.OtherAllowance = GetOtherAllowanceBM02(currentlstsalaryProcess[1]);

                        var oldWorkingProcesses = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = personId });
                        oldWorkingProcesses = oldWorkingProcesses.Where(x =>
                            x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                            && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                            && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower())
                            && x.FromDate >= currentlstsalaryProcess[1].IssuedTime
                            && x.FromDate < currentlstsalaryProcess.First().IssuedTime// kiểm tra ngày bắt đầu công tác phải sau ngày quyết định nâng lương
                            ).OrderByDescending(x => x.FromDate).ToList();
                        if (oldWorkingProcesses.Count > 0)
                        {
                            oldCoefficientsSalaryDetail.OrganName = oldWorkingProcesses.First().OrganName;
                            oldCoefficientsSalaryDetail.JobPosition = oldWorkingProcesses.First().JobPosition;
                            oldCoefficientsSalaryDetail.WorkingTitle = oldWorkingProcesses.First().WorkingTitle;
                        }

                        bm02Dto.oldCoefficientsSalaryDetail = oldCoefficientsSalaryDetail;
                    }
                    else
                    {
                        var oldlstsalaryProcess = lstsalaryProcess.Where(x => x.IssuedTime < currentlstsalaryProcess.First().IssuedTime)
                            .OrderByDescending(x => x.IssuedTime).ToList();
                        if(oldlstsalaryProcess.Count > 0)
                        {
                            oldCoefficientsSalaryDetail.Glone = oldlstsalaryProcess.First().Glone;
                            oldCoefficientsSalaryDetail.PayRate = oldlstsalaryProcess.First().PayRate;
                            oldCoefficientsSalaryDetail.CoefficientsSalary = oldlstsalaryProcess.First().CoefficientsSalary.ToString();
                            oldCoefficientsSalaryDetail.FromDate = oldlstsalaryProcess.First().SalaryIncreaseTime;
                            oldCoefficientsSalaryDetail.PositionAllowance = oldlstsalaryProcess.First()?.LeadershipPositionAllowance ?? "";
                            oldCoefficientsSalaryDetail.OtherAllowance = GetOtherAllowanceBM02(oldlstsalaryProcess.First());
                            // Tìm quá trình công tác theo theo thời gian của quyết định nâng lương
                            var oldworkingProcesses = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = personId });
                            oldworkingProcesses = oldworkingProcesses.Where(x =>
                                x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                                && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                                && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower())
                                && x.FromDate >= oldlstsalaryProcess.First().IssuedTime
                                && x.FromDate < currentlstsalaryProcess.First().IssuedTime // kiểm tra ngày bắt đầu công tác phải sau ngày quyết định nâng lương
                                ).OrderByDescending(x => x.FromDate).ToList();
                            if (oldworkingProcesses.Count > 0)
                            {
                                oldCoefficientsSalaryDetail.OrganName = oldworkingProcesses.First().OrganName;
                                oldCoefficientsSalaryDetail.JobPosition = oldworkingProcesses.First().JobPosition;
                                oldCoefficientsSalaryDetail.WorkingTitle = oldworkingProcesses.First().WorkingTitle;
                            }

                            bm02Dto.oldCoefficientsSalaryDetail = oldCoefficientsSalaryDetail;
                        }
                    }
                }
                bm02Dtos.Add(bm02Dto);
            }
            bm02Dtos = bm02Dtos.OrderBy(x => x.FullName.Trim().Split(" ").ToList().Last()).ToList();

            for (int i = 0; i < bm02Dtos.Count; i++)
            {
                bm02Dtos[i].STT = i + 1;
            }
            string orgName = GetOrgName(reportFilterDto.OrgId.Value);
            return await _reportExcelExporter.ExportBM02ToFile(bm02Dtos,fromDate,toDate, orgName);
        }

        public async Task<FileDto> GetBM03ListDto(ReportFilterDto input)
        {
            try
            {
                if(!input.OrgId.HasValue)
                {
                    throw new UserFriendlyException("Vui lòng chọn đơn vị");
                }
                var query = (from person in _personalProfileAppService.GetAllList().Result
                            join salaryProcess in _salaryProcessAppService.GetAllList().Result
                            .Where(u => u.SalaryIncreaseTime >= input.FromDate && u.SalaryIncreaseTime <= input.ToDate)
                            on person.Id equals salaryProcess.PersonId
                            join civil in _civilServantAppService.GetAllCivilServant().Result on person.CivilServantSector equals civil.Id
                            join politic in _categoryAppService.GetListByType(StatusEnum.CategoryType.PoliticsTheoReticalLevel).Result.Items
                            on person.PoliticsTheoReticalLevel equals politic.Id
                            join orgUnit in _organizationUnitStaffAppService.GetAll().Result.Where(u => u.OrganizationUnitId == input.OrgId) on person.Id equals orgUnit.StaffId
                            group salaryProcess by new { person, civil, politic } into staff
                            select new BM03Dto
                            {
                                FullName = staff.Key.person.FullName,
                                ServantCode = staff.Key.civil.Code,
                                IssuedDate = staff.OrderByDescending(u => u.IssuedTime).First().IssuedTime,
                                MeetQualificationRequirement = new MeetQualificationRequirement
                                {
                                    Specialize = staff.Key.person.Specialized,
                                    ForeignLanguage = staff.Key.person.ForeignLanguage,
                                    StateManagement = staff.Key.person.StateManagement,
                                    PoliticsTheoReticalLevel = staff.Key.politic.Title,
                                    InformationTechnology = staff.Key.person.InfomationTechnology,
                                    TimeKeepOldServant = GetAge.GetRealAge(staff.OrderByDescending(u => u.IssuedTime).First().IssuedTime).ToString() + " năm",
                                }
                            }).Select((u, i) => new BM03Dto
                            {
                                STT = i+1,
                                FullName = u.FullName,
                                ServantCode = u.ServantCode,
                                IssuedDate = u.IssuedDate,
                                MeetQualificationRequirement = u.MeetQualificationRequirement
                            }).ToList();
                var orgs = await _organizationUnitAppService.GetOrganizationUnits();
                var org = orgs.Items.Where(u => u.Id == input.OrgId).FirstOrDefault();
                return await _reportExcelExporter.ExportBM03ToFile(query, (DateTime)input.FromDate, (DateTime)input.ToDate, org.DisplayName);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM04ListDto(ReportFilterDto reportFilterDto)
        {
            if (!reportFilterDto.OrgId.HasValue)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn vị");
            }
            if (!reportFilterDto.FromDate.HasValue)
            {
                reportFilterDto.FromDate = DateTime.Now;
            }
            if (!reportFilterDto.ToDate.HasValue)
            {
                reportFilterDto.ToDate = DateTime.Now;
            }
            if (reportFilterDto.FromDate > reportFilterDto.ToDate)
            {
                throw new UserFriendlyException("Ngày kết thúc đang sớm hơn ngày bắt đầu");
            }
            var fromDate = reportFilterDto.FromDate.Value;
            var toDate = reportFilterDto.ToDate.Value;

            List<BM04Dto> bm04Dtos = new List<BM04Dto>();
            var lstStaffId = _workingProcessAppService.GetAllListStaffId();

            lstStaffId = (from l in _organizationUnitStaffAppService.GetAll().Result
                           join
                          p in lstStaffId on l.StaffId equals p
                           where reportFilterDto.OrgId.HasValue && l.OrganizationUnitId == reportFilterDto.OrgId
                           select l.StaffId).Distinct().ToList();

            foreach (var staffId in lstStaffId)
            {
                var allLstwps = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = staffId });

                var lstwps = allLstwps.Where(x => x.FromDate >= fromDate && x.FromDate <= toDate &&
                    (x.TypeOfChange.Trim().ToLower().Equals("Điều động".Trim().ToLower())
                    || x.TypeOfChange.Trim().ToLower().Equals("Luân chuyển".Trim().ToLower())
                    || x.TypeOfChange.Trim().ToLower().Equals("Biệt phái".Trim().ToLower()))
                ).OrderByDescending(x => x.FromDate).ToList();

                if(lstwps.Count > 0)
                {
                    BM04Dto bM04Dto = new BM04Dto();
                    var staffDetail = await _personalProfileAppService.Get(staffId);
                    bM04Dto.FullName = staffDetail.FullName;

                    var DieuDong = lstwps.Where(x =>
                            x.TypeOfChange.Trim().ToLower().Equals("Điều động".Trim().ToLower())
                        ).OrderByDescending(x => x.FromDate).FirstOrDefault();
                    if (DieuDong != null)
                    {
                        var oldWp = allLstwps.Where(x =>
                                x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                                && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                                && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower())
                                && x.FromDate < DieuDong.FromDate).OrderByDescending(x => x.FromDate).FirstOrDefault();
                        if (oldWp != null)
                        {
                            bM04Dto.OldJobPosition = oldWp.JobPosition;
                            if (!oldWp.ToDate.HasValue || DieuDong.FromDate <= oldWp.ToDate.Value)
                            {
                                var time = (DieuDong.FromDate.Year - oldWp.FromDate.Year) * 12 + DieuDong.FromDate.Month - oldWp.FromDate.Month;
                                bM04Dto.TimeHoldOldJobPosition = time > 12 ? $"{time / 12} năm, {time % 12} tháng" : $"{time} tháng";
                            }
                            else
                            {
                                var time = (oldWp.ToDate.Value.Year - oldWp.FromDate.Year) * 12 + oldWp.ToDate.Value.Month - oldWp.FromDate.Month;
                                bM04Dto.TimeHoldOldJobPosition = time > 12 ? $"{time / 12} năm, {time % 12} tháng" : $"{time} tháng";

                            }
                        }
                    }
                    var LuanChuyen = lstwps.Where(x =>
                            x.TypeOfChange.Trim().ToLower().Equals("Luân chuyển".Trim().ToLower())
                        ).OrderByDescending(x => x.FromDate).FirstOrDefault();
                    if(LuanChuyen != null)
                    {
                        bM04Dto.NewWorkingTitle = LuanChuyen.WorkingTitle;
                    }

                    var BietPhai = lstwps.Where(x =>
                        x.TypeOfChange.Trim().ToLower().Equals("Biệt phái".Trim().ToLower())
                    ).OrderByDescending(x => x.FromDate).FirstOrDefault();
                    if (BietPhai != null)
                    {
                        if (BietPhai.ToDate.HasValue)
                        {
                            var time = (BietPhai.ToDate.Value.Year - BietPhai.FromDate.Year) * 12 + BietPhai.ToDate.Value.Month - BietPhai.FromDate.Month;
                            bM04Dto.SecondedTime = time > 12 ? $"{time / 12} năm, {time % 12} tháng" : $"{time} tháng";
                        }
                        else bM04Dto.SecondedTime = "Chưa xác định";
                    }

                    var latest = lstwps.First();
                    bM04Dto.DecisionYear = latest.IssuedDate?.Year;
                    bM04Dto.DecisionMaker = latest.DecisionMaker;

                    var latestDate = latest.FromDate;

                    var earlierWp = allLstwps.Where(x =>
                        x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                        && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                        && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower())
                        && x.FromDate < latestDate
                    ).OrderByDescending(x => x.FromDate).FirstOrDefault();

                    if(earlierWp != null)
                    {
                        if(latest.TypeOfChange.Trim().ToLower().Equals("Điều động".Trim().ToLower()))
                        {
                            bM04Dto.OldAndNewWorkiingTitle = $"- Trước điều động: {earlierWp.WorkingTitle}\n- Sau điều động: {latest.WorkingTitle}";
                        }
                        if(latest.TypeOfChange.Trim().ToLower().Equals("Luân chuyển".Trim().ToLower()))
                        {
                            bM04Dto.OldAndNewWorkiingTitle = $"- Trước luân chuyển: {earlierWp.WorkingTitle}\n- Sau luân chuyển: {latest.WorkingTitle}";
                        }                        
                        if(latest.TypeOfChange.Trim().ToLower().Equals("Biệt phái".Trim().ToLower()))
                        {
                            bM04Dto.OldAndNewWorkiingTitle = $"- Trước biệt phái: {earlierWp.WorkingTitle}\n- Sau biệt phái: {latest.WorkingTitle}";
                        }
                    }
                    else
                    {
                        if (latest.TypeOfChange.Trim().ToLower().Equals("Điều động".Trim().ToLower()))
                        {
                            bM04Dto.OldAndNewWorkiingTitle = $"- Sau điều động: {latest.WorkingTitle}";
                        }
                        if (latest.TypeOfChange.Trim().ToLower().Equals("Luân chuyển".Trim().ToLower()))
                        {
                            bM04Dto.OldAndNewWorkiingTitle = $"- Sau luân chuyển: {latest.WorkingTitle}";
                        }
                        if (latest.TypeOfChange.Trim().ToLower().Equals("Biệt phái".Trim().ToLower()))
                        {
                            bM04Dto.OldAndNewWorkiingTitle = $"- Sau biệt phái: {latest.WorkingTitle}";
                        }
                    }

                    var currentlstsalaryProcess = _salaryProcessAppService.GetAll(staffId).Result.Where(x => x.IssuedTime <= latestDate)
                        .OrderByDescending(x => x.IssuedTime).FirstOrDefault();

                    if(currentlstsalaryProcess != null)
                    {
                        var earlierSalary = _salaryProcessAppService.GetAll(staffId).Result.Where(x => x.IssuedTime < currentlstsalaryProcess.IssuedTime)
                        .OrderByDescending(x => x.IssuedTime).FirstOrDefault();
                        if(earlierSalary != null && earlierSalary.Glone != currentlstsalaryProcess.Glone)
                        {
                            bM04Dto.Glone = $"- Trước : {earlierSalary.Glone}\n- Sau : {currentlstsalaryProcess.Glone}";
                            bM04Dto.Allowance = GetAllowanceBM04(currentlstsalaryProcess, earlierSalary);
                        }
                        else
                        {
                            bM04Dto.Glone = $"{currentlstsalaryProcess.Glone}";
                            bM04Dto.Allowance = GetAllowanceBM04(currentlstsalaryProcess, earlierSalary);
                        }
                    }
                    bm04Dtos.Add(bM04Dto);
                }
            }

            bm04Dtos = bm04Dtos.OrderBy(x => x.FullName.Trim().Split(" ").ToList().Last()).ToList();

            for (int i = 0; i < bm04Dtos.Count; i++)
            {
                bm04Dtos[i].STT = i + 1;
            }
            string orgName = GetOrgName(reportFilterDto.OrgId.Value);
            return await _reportExcelExporter.ExportBM04ToFile(bm04Dtos, fromDate, toDate, orgName);
        }

        public async Task<FileDto> GetBM05ListDto(ReportFilterDto input)
        {
            try
            {
                if (!input.OrgId.HasValue)
                {
                    throw new UserFriendlyException("Vui lòng chọn đơn vị");
                }
                var query = (from person in _personalProfileAppService.GetAllList().Result
                             join workingProcess in _workingProcessAppService.GetAllList().Result
                             .Where(u => u.FromDate >= input.FromDate && u.FromDate <= input.ToDate)
                             on person.Id equals workingProcess.PersonId
                             join form in _categoryAppService.GetListByType(StatusEnum.CategoryType.WorkingProcessForm).Result.Items
                             .Where(u => u.Title.Trim().ToLower() == ("Bổ nhiệm lại").Trim().ToLower() || u.Title.Trim().ToLower() == ("Bổ nhiệm").Trim().ToLower())
                             on workingProcess.TypeOfChangeId equals form.Id
                             join position in _categoryAppService.GetListPosition().Result.Where(u => u.CategoryType == StatusEnum.CategoryType.LeadershipTitle) on workingProcess.WorkingTitleId equals position.Id
                             join decisionMaker in _categoryAppService.GetListByType(StatusEnum.CategoryType.DecisionMaker).Result.Items on workingProcess.DecisionMakerId equals decisionMaker.Id
                             join qualification in _categoryAppService.GetListByType(StatusEnum.CategoryType.Diploma).Result.Items on person.HighestAcademicLevel equals qualification.Id
                             join politicsTheoReticalLevel in _categoryAppService.GetListByType(StatusEnum.CategoryType.PoliticsTheoReticalLevel).Result.Items on person.PoliticsTheoReticalLevel equals politicsTheoReticalLevel.Id
                             join orgUnit in _organizationUnitStaffAppService.GetAll().Result.Where(u => u.OrganizationUnitId == input.OrgId) on person.Id equals orgUnit.StaffId
                             join propertyDeclaration in _propertyDeclarationAppService.GetAll(new PropertyDeclarations.Dto.GetAllFilter { PersonId = null }).Result
                             .Where(u => u.Year >= input.FromDate.Value.Year && u.Year <= input.ToDate.Value.Year)
                             on person.Id equals propertyDeclaration.PersonId into prop
                             from decla in prop.DefaultIfEmpty()
                             group new { workingProcess, decla } by new { person, position, decisionMaker, qualification, politicsTheoReticalLevel, form } into staff
                             select new BM05Dto
                             {
                                 FullName = staff.Key.person.FullName,
                                 Male = staff.Key.person.Gender == Gender.Male ? staff.Key.person.DateOfBirth.Year.ToString() : null,
                                 FeMale = staff.Key.person.Gender == Gender.Female ? staff.Key.person.DateOfBirth.Year.ToString() : null,
                                 ForeignLanguage = staff.Key.person.ForeignLanguage,
                                 InformationTechnology = staff.Key.person.InfomationTechnology,
                                 Position = staff.Key.position.Title,
                                 DecisionMaker = staff.Key.decisionMaker.Title,
                                 CurriculumVitae = staff.Key.person.CurriculumVitae.Value == StatusEnum.BoolEnum.Yes ? "x" : "0",
                                 PropertyDeclaration = staff.Select(u => u.decla).FirstOrDefault() != null
                                 ? (staff.Select(u => u.decla).OrderByDescending(u => u.Year).First().IsExist == StatusEnum.PropertyDeclarationBoolConst.Yes ? "x"
                                 : "0") : "0",
                                 Qualification = staff.Key.qualification.Title,
                                 PoliticsTheoReticalLevel = staff.Key.politicsTheoReticalLevel.Title,
                                 StateManagement = staff.Key.person.StateManagement,
                                 DecisionAppointDate = staff.Key.form.Title.Trim().ToLower() == ("Bổ nhiệm").Trim().ToLower() ? staff.OrderByDescending(u => u.workingProcess.FromDate).Select(u => u.workingProcess).First().FromDate : (DateTime?)null,
                                 DecisionReAppointDate = staff.Key.form.Title.Trim().ToLower() == ("Bổ nhiệm lại").Trim().ToLower() ? staff.OrderByDescending(u => u.workingProcess.FromDate).Select(u => u.workingProcess).First().FromDate : (DateTime?)null,
                             }).Select((u, i) => new BM05Dto
                             {
                                 STT = i + 1,
                                 FullName = u.FullName,
                                 Male = u.Male,
                                 FeMale = u.FeMale,
                                 ForeignLanguage = u.ForeignLanguage,
                                 InformationTechnology = u.InformationTechnology,
                                 Position = u.Position,
                                 DecisionMaker = u.DecisionMaker,
                                 CurriculumVitae = u.CurriculumVitae,
                                 PropertyDeclaration = u.PropertyDeclaration,
                                 Qualification = u.Qualification,
                                 PoliticsTheoReticalLevel = u.PoliticsTheoReticalLevel,
                                 StateManagement = u.StateManagement,
                                 DecisionAppointDate = u.DecisionAppointDate,
                                 DecisionReAppointDate = u.DecisionReAppointDate,
                             }).ToList();
                var orgs = await _organizationUnitAppService.GetOrganizationUnits();
                var org = orgs.Items.Where(u => u.Id == input.OrgId).FirstOrDefault();
                return await _reportExcelExporter.ExportBM05ToFile(query, (DateTime)input.FromDate, (DateTime)input.ToDate, org.DisplayName);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM07ListDto(ReportFilterDto reportFilterDto)
        {
            if (!reportFilterDto.OrgId.HasValue)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn vị");
            }
            if (!reportFilterDto.FromDate.HasValue)
            {
                reportFilterDto.FromDate = DateTime.Now;
            }
            if (!reportFilterDto.ToDate.HasValue)
            {
                reportFilterDto.ToDate = DateTime.Now;
            }
            if (reportFilterDto.FromDate > reportFilterDto.ToDate)
            {
                throw new UserFriendlyException("Ngày kết thúc đang sớm hơn ngày bắt đầu");
            }
            var fromDate = reportFilterDto.FromDate.Value;
            var toDate = reportFilterDto.ToDate.Value;
            List<BM07Dto> bm07Dtos = new List<BM07Dto>();
            var lstStaffId = _workingProcessAppService.GetAllListStaffId();

            lstStaffId = (from l in _organizationUnitStaffAppService.GetAll().Result
                          join
                         p in lstStaffId on l.StaffId equals p
                          where reportFilterDto.OrgId.HasValue && l.OrganizationUnitId == reportFilterDto.OrgId
                          select l.StaffId).Distinct().ToList();
            foreach (var staffId in lstStaffId)
            {
                var allLstwps = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = staffId });

                var wp = allLstwps.Where(x => x.FromDate >= fromDate && x.FromDate <= toDate
                    && (x.TypeOfChange.Trim().ToLower().Equals("Miễn nhiệm".Trim().ToLower())
                    || x.TypeOfChange.Trim().ToLower().Equals("Từ chức".Trim().ToLower()))
                ).OrderByDescending(x => x.FromDate).FirstOrDefault();

                if (wp != null)
                {
                    BM07Dto bM07Dto = new BM07Dto();
                    var staffDetail = await _personalProfileAppService.Get(staffId);
                    bM07Dto.FullName = staffDetail.FullName;
                    bM07Dto.IssuedDate = wp.FromDate;
                    bM07Dto.DecisionMaker = wp.DecisionMaker;
                    bM07Dto.WorkingTitleAndOrg = $"{wp.WorkingTitle}, {wp.OrganName}";

                    var oldWp = allLstwps.Where(x =>
                                x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                                && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                                && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower())
                                && x.FromDate < wp.FromDate).OrderByDescending(x => x.FromDate).FirstOrDefault();
                    if (oldWp != null)
                    {
                        bM07Dto.OldWorkingTitleAndOrg = $"{oldWp.WorkingTitle}, {oldWp.OrganName}";
                        if (!oldWp.ToDate.HasValue || oldWp.ToDate.Value > Clock.Now)
                        {
                            var time = (Clock.Now.Year - oldWp.FromDate.Year) * 12 + Clock.Now.Month - oldWp.FromDate.Month;
                            bM07Dto.TimeHoldWorkingTitle = time > 12 ? $"{time / 12} năm, {time % 12} tháng" : $"{time} tháng";
                        }
                        else
                        {
                            var time = (oldWp.ToDate.Value.Year - oldWp.FromDate.Year) * 12 + oldWp.ToDate.Value.Month - oldWp.FromDate.Month;
                            bM07Dto.TimeHoldWorkingTitle = time > 12 ? $"{time / 12} năm, {time % 12} tháng" : $"{time} tháng";

                        }
                    }

                    bm07Dtos.Add(bM07Dto);
                }
            }

            bm07Dtos = bm07Dtos.OrderBy(x => x.FullName.Trim().Split(" ").ToList().Last()).ToList();

            for (int i = 0; i < bm07Dtos.Count; i++)
            {
                bm07Dtos[i].STT = i + 1;
            }
            string orgName = GetOrgName(reportFilterDto.OrgId.Value);
            return await _reportExcelExporter.ExportBM07ToFile(bm07Dtos, fromDate, toDate, orgName);
        }

        public async Task<FileDto> GetBM08ListDto(ReportFilterDto reportFilterDto)
        {
            if (!reportFilterDto.OrgId.HasValue)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn vị");
            }
            try
            {
                var bm08Dtos = (from pT in (from person in _personalProfileAppService.GetAllList().Result
                                join orgUnit in _organizationUnitStaffAppService.GetAll().Result.Where(u => u.OrganizationUnitId == reportFilterDto.OrgId) on person.Id equals orgUnit.StaffId
                                join trainningInfo in _trainningInfoAppService.GetAllList().Result.Where(u => u.FromDate >= reportFilterDto.FromDate && u.FromDate <= reportFilterDto.ToDate)
                                on person.Id equals trainningInfo.PersonId into trainningGroup
                                where trainningGroup.ToList().Count > 0
                                select new
                                {
                                    Id = person.Id,
                                    FullName = person.FullName,
                                    TrainningInfoes = trainningGroup
                                }).ToList()
                               join workingP in _workingProcessAppService.GetAllListDetail().Result.Where(
                                   x =>
                                        x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                                        && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                                        && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower())
                                   ).OrderByDescending(x => x.FromDate) on pT.Id equals workingP.PersonId
                               into wpG from wp in wpG.DefaultIfEmpty(new GetWorkingProcessDto { WorkingTitle = string.Empty})
                               group wp by pT into ptW
                               join salaryP in _salaryProcessAppService.GetAllList().Result.OrderByDescending(x => x.IssuedTime) on ptW.Key.Id equals salaryP.PersonId
                               into sG from s in sG.DefaultIfEmpty(new SalaryProcesses.Dtos.SalaryProcessDto { Glone = string.Empty})
                               group s by ptW into ptwS
                               select new BM08Dto
                               {
                                   FullName = ptwS.Key.Key.FullName,
                                   WorkingTitle = ptwS.Key.FirstOrDefault().WorkingTitle,
                                   Org = ptwS.Key.FirstOrDefault().OrganName,
                                   Glone = ptwS.FirstOrDefault().Glone,
                                   TrainningDetails = ptwS.Key.Key.TrainningInfoes.OrderByDescending(x => x.FromDate).Select(x =>
                                        new TrainningDetail
                                        {
                                            TrainningContent = x.MajoringName,
                                            TrainningType = x.TrainningType,
                                            TrainningTime = $"Từ {Clock.Normalize(x.FromDate).ToShortDateString()} \nđến {Clock.Normalize(x.ToDate).ToShortDateString()}"
                                        }).ToList()
                               }).ToList();

                bm08Dtos = bm08Dtos.OrderBy(x => x.FullName.Trim().Split(" ").ToList().Last()).ToList();

                for (int i = 0; i < bm08Dtos.Count; i++)
                {
                    bm08Dtos[i].STT = i + 1;
                }
                string orgName = GetOrgName(reportFilterDto.OrgId.Value);
                return await _reportExcelExporter.ExportBM08ToFile(bm08Dtos, (DateTime)reportFilterDto.FromDate, (DateTime)reportFilterDto.ToDate,orgName);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM09ListDto(ReportFilterDto input)
        {
            if (!input.OrgId.HasValue)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn vị");
            }
            try
            {
                int now = DateTime.Today.Year;
                int yearAgo = now - 1;
                var query = (from person in _personalProfileAppService.GetAllList().Result
                             join org in _organizationUnitStaffAppService.GetAll().Result.Where(u => u.OrganizationUnitId == input.OrgId)
                             on person.Id equals org.StaffId
                             group org by person into staff
                             select new
                             {
                                 Id = staff.Key.Id,
                                 FullName = staff.Key.FullName
                             }).ToList();
                int stt = 1;
                var result = new List<BM09Dto>();
                foreach(var person in query)
                {
                    var listAssessed = new List<GetAssessedByYearDto>();
                    var assessedCivil = await _assessedByYearAppService.GetAll(new AssessFilter { PersonId = person.Id, Type = StatusEnum.AssessedByYearEnum.CivilServant });
                    var assessedPublic = await _assessedByYearAppService.GetAll(new AssessFilter { PersonId = person.Id, Type = StatusEnum.AssessedByYearEnum.PublicServant });
                    listAssessed.AddRange(assessedCivil);
                    listAssessed.AddRange(assessedPublic);
                    var workingProcess = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter { PersonId = person.Id });
                    if(listAssessed.Where(u => u.Year == now.ToString() || u.Year == yearAgo.ToString()).Count() > 0)
                    {
                        var obj = new BM09Dto
                        {
                            STT = stt,
                            FullName = person.FullName
                        };
                        var listAssessed1 = listAssessed.Where(u => u.Year == yearAgo.ToString()).FirstOrDefault();
                        var listAssessed2 = listAssessed.Where(u => u.Year == now.ToString()).FirstOrDefault();
                        if (listAssessed1 != null)
                        {
                            var workingP1 = workingProcess.Where(u => u.FromDate.Year <= yearAgo && u.ToDate.Value.Year >= yearAgo).OrderByDescending(u => u.FromDate).FirstOrDefault();
                            var evaluate1 = new Evaluate
                            {
                                Position = workingP1 != null ? workingP1.WorkingTitle : "",
                                SelfAssessment = listAssessed1.SelfAssessment,
                                LeaderAssessment = listAssessed1.AssessmentByLeader,
                                GroupAssessment = listAssessed1.CollectiveFeedback,
                                CmpetentPersonsAssessment = listAssessed1.EvaluationOfAuthorizedPerson,
                                Result = listAssessed1.ResultsOfClassification
                            };
                            obj.Evaluate1 = evaluate1;
                        }
                        if(listAssessed2 != null)
                        {
                            var workingP2 = workingProcess.Where(u => u.FromDate.Year <= now && u.ToDate.Value.Year >= now).OrderByDescending(u => u.FromDate).FirstOrDefault();
                            var evaluate2 = new Evaluate
                            {
                                Position = workingP2 != null ? workingP2.WorkingTitle : "",
                                SelfAssessment = listAssessed2.SelfAssessment,
                                LeaderAssessment = listAssessed2.AssessmentByLeader,
                                GroupAssessment = listAssessed2.CollectiveFeedback,
                                CmpetentPersonsAssessment = listAssessed2.EvaluationOfAuthorizedPerson,
                                Result = listAssessed2.ResultsOfClassification
                            };
                            obj.Evaluate2 = evaluate2;
                        }
                        result.Add(obj);
                        stt++;
                    }
                }
                var orgs = await _organizationUnitAppService.GetOrganizationUnits();
                var orga = orgs.Items.Where(u => u.Id == input.OrgId).FirstOrDefault();

                return await _reportExcelExporter.ExportBM09ToFile(result, yearAgo, now, orga.DisplayName);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM10ListDto(ReportFilterDto reportFilterDto)
        {
            if (!reportFilterDto.OrgId.HasValue)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn vị");
            }
            try
            {
                List<BM10Dto> lstBM10Dtos = new List<BM10Dto>();
                var lstStaffId = _commendationAppService.GetAllList().Result.Select(x=>x.PersonId);

                lstStaffId = (from l in _organizationUnitStaffAppService.GetAll().Result
                              join
                             p in lstStaffId on l.StaffId equals p
                              where reportFilterDto.OrgId.HasValue && l.OrganizationUnitId == reportFilterDto.OrgId
                              select l.StaffId).Distinct().ToList();
                foreach(var staffId in lstStaffId)
                {
                    BM10Dto bM10Dto = new BM10Dto();
                    var staffDetail = await _personalProfileAppService.Get(staffId);
                    bM10Dto.FullName = staffDetail.FullName;
                    var commendation1 = _commendationAppService.GetAll(new Commendations.Dto.GetAllFilter { PersonId = staffId }).Result.Where(x=>x.CommendationYear == Clock.Now.Year).FirstOrDefault();
                    if(commendation1 != null)
                    {
                        bM10Dto.CommendationTitle1 = commendation1.CommendationTitle;
                    }
                    var commendation2 = _commendationAppService.GetAll(new Commendations.Dto.GetAllFilter { PersonId = staffId }).Result.Where(x => x.CommendationYear == Clock.Now.Year - 1).FirstOrDefault();
                    if (commendation2 != null)
                    {
                        bM10Dto.CommendationTitle2 = commendation2.CommendationTitle;
                    }

                    var commendation3 = _commendationAppService.GetAll(new Commendations.Dto.GetAllFilter { PersonId = staffId }).Result.Where(x => x.CommendationYear == Clock.Now.Year - 2).FirstOrDefault();
                    if (commendation3 != null)
                    {
                        bM10Dto.CommendationTitle3 = commendation3.CommendationTitle;
                    }
                    lstBM10Dtos.Add(bM10Dto);
                }

                lstBM10Dtos = lstBM10Dtos.OrderBy(x => x.FullName.Trim().Split(" ").ToList().Last()).ToList();

                for (int i = 0; i < lstBM10Dtos.Count; i++)
                {
                    lstBM10Dtos[i].STT = i + 1;
                }

                int count1 = _commendationAppService.GetAllList().Result.Where(u => u.CommendationYear == Clock.Now.Year).GroupBy(x=>x.PersonId).ToList().Count;
                int count2 = _commendationAppService.GetAllList().Result.Where(u => u.CommendationYear == Clock.Now.Year-1).GroupBy(x => x.PersonId).ToList().Count;
                int count3 = _commendationAppService.GetAllList().Result.Where(u => u.CommendationYear == Clock.Now.Year-2).GroupBy(x => x.PersonId).ToList().Count;
                string orgName = GetOrgName(reportFilterDto.OrgId.Value);
                return await _reportExcelExporter.ExportBM10ToFile(lstBM10Dtos,count1,count2,count3, orgName);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM12ListDto(ReportFilterDto input)
        {
            if (!input.OrgId.HasValue)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn vị");
            }
            try
            {
                var query = (from person in _personalProfileAppService.GetAllList().Result
                             join org in _organizationUnitStaffAppService.GetAll().Result.Where(u => u.OrganizationUnitId == input.OrgId)
                             on person.Id equals org.StaffId
                             group org by person into staff
                             select new
                             {
                                 Id = staff.Key.Id,
                                 FullName = staff.Key.FullName,
                                 Gender = staff.Key.Gender,
                                 DateOfBirth = staff.Key.DateOfBirth,
                                 Male = staff.Key.Gender == Gender.Male ? staff.Key.DateOfBirth : (DateTime?)(null),
                                 FeMale = staff.Key.Gender == Gender.Female ? staff.Key.DateOfBirth : (DateTime?)(null)
                             }).ToList();
                var result = new List<BM12Dto>();
                int stt = 1;
                foreach(var person in query)
                {
                    var workingProcess = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter { PersonId = person.Id });
                    var retirement = workingProcess.Where(u => u.FromDate >= input.FromDate && u.FromDate <= input.ToDate
                    && u.TypeOfChange.Trim().ToLower() == ("Nghỉ hưu").Trim().ToLower() || u.TypeOfChange.Trim().ToLower() == ("Thôi việc").Trim().ToLower())
                        .OrderByDescending(u => u.FromDate).ToList();
                    var working = workingProcess.Where(u => u.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu").Trim().ToLower() && u.TypeOfChange.Trim().ToLower() != ("Thôi việc").Trim().ToLower())
                        .OrderByDescending(u => u.FromDate).FirstOrDefault();
                    if (retirement.Count > 0)
                    {
                        var obj = new BM12Dto
                        {
                            STT = stt,
                            FullName = person.FullName,
                            Male = person.Male,
                            FeMale = person.FeMale,
                            DecisionDate = retirement.First().IssuedDate ,
                            Position = working != null ? working.OrganName + ", " + working.WorkingTitle : "",
                            Retirement = new Retirement
                            {
                                NoticeDate = retirement.First().IssuedDate,
                                RetirementTimeInNotice = retirement.First().FromDate,
                                ToRetirementAge = CheckRetirement.CheckRetire2(person.Gender, person.DateOfBirth, retirement.First().FromDate) >= retirement.First().FromDate ? "x" : "",
                            },
                            Decision = new Decision
                            {
                                DecisionDate = retirement.First().IssuedDate,
                                TitlePersonDecision = retirement.First().DecisionMaker,
                                RetirementTime = retirement.First().FromDate
                            }
                        };
                        result.Add(obj);
                        stt++;
                    }
                }
                var orgs = await _organizationUnitAppService.GetOrganizationUnits();
                var orga = orgs.Items.Where(u => u.Id == input.OrgId).FirstOrDefault();
                return await _reportExcelExporter.ExportBM12ToFile(result, (DateTime)input.FromDate, (DateTime)input.ToDate, orga.DisplayName);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM15ListDto(ReportFilterDto input)
        {
            try
            {
                if (!input.OrgId.HasValue)
                {
                    throw new UserFriendlyException("Vui lòng chọn đơn vị");
                }
                var query = (from person in _personalProfileAppService.GetAllList().Result
                             join org in _organizationUnitStaffAppService.GetAll().Result
                             .Where(u => u.OrganizationUnitId == input.OrgId) on person.Id equals org.StaffId
                             join prop in _propertyDeclarationAppService.GetAll(new GetAllFilter { PersonId = null }).Result
                             .Where(u => u.Year == DateTime.Today.Year)
                             on person.Id equals prop.PersonId into p
                             from propertyD in p.DefaultIfEmpty()
                             group propertyD by person into staff
                             select new
                             {
                                 FullName = staff.Key.FullName,
                                 StaffBackground = staff.Key.StaffBackground == BoolEnum.Yes ? "x" : (staff.Key.StaffBackground == BoolEnum.No ? "0" : "thiếu"),
                                 CurriculumVitae = staff.Key.CurriculumVitae == BoolEnum.Yes ? "x" : (staff.Key.CurriculumVitae == BoolEnum.No ? "0" : "thiếu"),
                                 AdditionalStaffBackground = staff.Key.AdditionalStaffBackground == BoolEnum.Yes ? "x" : (staff.Key.AdditionalStaffBackground == BoolEnum.No ? "0" : "thiếu"),
                                 BriefBiography = staff.Key.BriefBiography == BoolEnum.Yes ? "x" : (staff.Key.BriefBiography == BoolEnum.No ? "0" : "thiếu"),
                                 BirthCertificate = staff.Key.BirthCertificate == BoolEnum.Yes ? "x" : (staff.Key.BirthCertificate == BoolEnum.No ? "0" : "thiếu"),
                                 HealthCertificate = staff.Key.HealthCertificate == BoolEnum.Yes ? "x" : (staff.Key.HealthCertificate == BoolEnum.No ? "0" : "thiếu"),
                                 PersonalIdentityDcuments = staff.Key.PersonalIdentityDcuments == BoolEnum.Yes ? "x" : (staff.Key.PersonalIdentityDcuments == BoolEnum.No ? "0" : "thiếu"),
                                 TrainingQualificationCV = staff.Key.TrainingQualificationCV == BoolEnum.Yes ? "x" : (staff.Key.TrainingQualificationCV == BoolEnum.No ? "0" : "thiếu"),
                                 RecruitmentDecision = staff.Key.RecruitmentDecision == BoolEnum.Yes ? "x" : (staff.Key.RecruitmentDecision == BoolEnum.No ? "0" : "thiếu"),
                                 DocumentsAppointingPosition = staff.Key.DocumentsAppointingPosition == BoolEnum.Yes ? "x" : (staff.Key.SelfAssessment == BoolEnum.No ? "0" : "thiếu"),
                                 SelfAssessment = staff.Key.SelfAssessment == BoolEnum.Yes ? "x" : (staff.Key.SelfAssessment == BoolEnum.No ? "0" : "thiếu"),
                                 PropertyDeclaration = staff.FirstOrDefault() != null ? (staff.FirstOrDefault().IsExist == PropertyDeclarationBoolConst.Yes ? "x" : "0") : "thiếu",
                                 EvaluationComment = staff.Key.EvaluationComment == BoolEnum.Yes ? "x" : (staff.Key.EvaluationComment == BoolEnum.No ? "0" : "thiếu"),
                                 HandlingDocument = staff.Key.HandlingDocument == BoolEnum.Yes ? "x" : (staff.Key.HandlingDocument == BoolEnum.No ? "0" : "thiếu"),
                                 WorkProcessDocument = staff.Key.WorkProcessDocument == BoolEnum.Yes ? "x" : (staff.Key.WorkProcessDocument == BoolEnum.No ? "0" : "thiếu"),
                                 RequestFormToResearchRecord = staff.Key.RequestFormToResearchRecord == BoolEnum.Yes ? "x" : (staff.Key.RequestFormToResearchRecord == BoolEnum.No ? "0" : "thiếu"),
                                 DocumentObjective = staff.Key.DocumentObjective == BoolEnum.Yes ? "x" : (staff.Key.DocumentObjective == BoolEnum.No ? "0" : "thiếu"),
                                 DocumentClipCover = staff.Key.DocumentClipCover == BoolEnum.Yes ? "x" : (staff.Key.DocumentClipCover == BoolEnum.No ? "0" : "thiếu"),
                                 ProfileCover = staff.Key.ProfileCover == BoolEnum.Yes ? "x" : (staff.Key.ProfileCover == BoolEnum.No ? "0" : "thiếu")
                             }).Select((u, i) => new BM15Dto
                             {
                                 STT = i + 1,
                                 FullName = u.FullName,
                                 StaffBackground = u.StaffBackground,
                                 CurriculumVitae = u.CurriculumVitae,
                                 AdditionalStaffBackground = u.AdditionalStaffBackground,
                                 BriefBiography = u.BriefBiography,
                                 BirthCertificate = u.BirthCertificate,
                                 HealthCertificate = u.HealthCertificate,
                                 PersonalIdentityDcuments = u.PersonalIdentityDcuments,
                                 TrainingQualificationCV = u.TrainingQualificationCV,
                                 RecruitmentDecision = u.RecruitmentDecision,
                                 DocumentsAppointingPosition = u.DocumentsAppointingPosition,
                                 SelfAssessment = u.SelfAssessment,
                                 PropertyDeclaration = u.PropertyDeclaration,
                                 EvaluationComment = u.EvaluationComment,
                                 HandlingDocument = u.HandlingDocument,
                                 WorkProcessDocument = u.WorkProcessDocument,
                                 RequestFormToResearchRecord = u.RequestFormToResearchRecord,
                                 DocumentObjective = u.DocumentObjective,
                                 DocumentClipCover = u.DocumentClipCover,
                                 ProfileCover = u.ProfileCover
                             }).ToList();
                var orgs = await _organizationUnitAppService.GetOrganizationUnits();
                var orga = orgs.Items.Where(u => u.Id == input.OrgId).FirstOrDefault();
                return await _reportExcelExporter.ExportBM15ToFile(query, orga.DisplayName);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetBM13ListDto(ReportFilterDto reportFilterDto)
        {
            if (!reportFilterDto.OrgId.HasValue)
            {
                throw new UserFriendlyException("Vui lòng chọn đơn vị");
            }

            if (!reportFilterDto.FromDate.HasValue)
            {
                reportFilterDto.FromDate = DateTime.Now;
            }
            if (!reportFilterDto.ToDate.HasValue)
            {
                reportFilterDto.ToDate = DateTime.Now;
            }
            if (reportFilterDto.FromDate > reportFilterDto.ToDate)
            {
                throw new UserFriendlyException("Ngày kết thúc đang sớm hơn ngày bắt đầu");
            }
            var fromDate = reportFilterDto.FromDate.Value;
            var toDate = reportFilterDto.ToDate.Value;
            try
            {
                List<BM13Dto> lstBM13Dtos = new List<BM13Dto>();
                var lstStaffId = _recruitmentInfomationAppService.GetAllList().Result.Items.Select(x => x.ProfileStaffId);

                lstStaffId = (from l in _organizationUnitStaffAppService.GetAll().Result
                              join
                             p in lstStaffId on l.StaffId equals p
                              where reportFilterDto.OrgId.HasValue && l.OrganizationUnitId == reportFilterDto.OrgId
                              select l.StaffId).Distinct().ToList();
                foreach (var staffId in lstStaffId)
                {
                    BM13Dto dto = new BM13Dto();
                    var staffDetail = await _personalProfileAppService.Get(staffId);
                    dto.FullName = staffDetail.FullName;

                    var r = _recruitmentInfomationAppService.GetAll(staffId).Result.Items.Where(x => x.TimeGetJob >= fromDate && x.TimeGetJob <= toDate ).FirstOrDefault();
                    if(r != null)
                    {
                        dto.DonDuTuyen = r.RegistrationForm == BoolConst.Yes ? "x": EnumExtensions.GetDisplayName(r.RegistrationForm);
                        dto.LyLichCoXacNhan = r.CertifiedBackground == BoolConst.Yes ? "x" : EnumExtensions.GetDisplayName(r.CertifiedBackground);
                        dto.BanSaoKhaiSinh = r.CopyOfBirthCertificate == BoolConst.Yes ? "x" : EnumExtensions.GetDisplayName(r.CopyOfBirthCertificate);
                        dto.GiayChungNhanSucKhoe = r.HealthCertificate == BoolConst.Yes ? "x" : EnumExtensions.GetDisplayName(r.HealthCertificate);
                        dto.ChungNhanDoiTuongUuTien = r.PreferredCertificate;
                        dto.KhongBiKyLuat = r.NotDisciplined == BoolConst.Yes ? "x" : EnumExtensions.GetDisplayName(r.NotDisciplined);
                        dto.ChuyenMon = r.Expertise;
                        dto.NgoaiNgu = r.OtherLanguage;
                        dto.TinHoc = r.InfomationTechnology;
                        dto.ThoiGianThongBaoKetQua = Clock.Normalize(r.TimeElectNotice).ToShortDateString();
                        dto.ThoiGianRaQuyetDinh = Clock.Normalize(r.TimeDecision).ToShortDateString();
                        dto.DonViVaViTriCongTac = r.WorkUnit;
                        dto.ThoiGianDenNhanViec = Clock.Normalize(r.TimeGetJob).ToShortDateString();
                        dto.QuyetDinhHuongDanTapSu = r.RegulationsGuideProbation;
                        dto.TapSuCheDoCuaNguoiTapSu = r.RegimeOfApprentice == BoolConst.Yes ? "x": EnumExtensions.GetDisplayName(r.RegimeOfApprentice);
                        dto.CheDoCuaNguoiHuongDan = r.RegimeOfApprenticeInstructor;
                        dto.MienTapSu = r.ExemptProbationary == BoolConst.Yes ? "x" : EnumExtensions.GetDisplayName(r.ExemptProbationary);
                    }

                    lstBM13Dtos.Add(dto);
                }

                lstBM13Dtos = lstBM13Dtos.OrderBy(x => x.FullName.Trim().Split(" ").ToList().Last()).ToList();

                for (int i = 0; i < lstBM13Dtos.Count; i++)
                {
                    lstBM13Dtos[i].STT = i + 1;
                }
                string orgName = GetOrgName(reportFilterDto.OrgId.Value);
                return await _reportExcelExporter.ExportBM13ToFile(lstBM13Dtos,fromDate,toDate, orgName);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        private string GetOtherAllowanceBM02(SalaryProcessDto dto)
        {
            string result = "";
            if (!string.IsNullOrEmpty(dto.ToxicAllowance))
            {
                result = result + $"Phụ cấp độc hại: {dto.ToxicAllowance}\n";
            }
            if (!string.IsNullOrEmpty(dto.AreaAllowance))
            {
                result = result + $"Phụ cấp khu vực: {dto.AreaAllowance}\n";
            }
            if (!string.IsNullOrEmpty(dto.ResponsibilityAllowance))
            {
                result = result + $"Phụ cấp trách nhiệm: {dto.ResponsibilityAllowance}\n";
            }
            if (!string.IsNullOrEmpty(dto.MobileAllowance))
            {
                result = result + $"Phụ cấp lưu động: {dto.MobileAllowance}\n";
            }
            return result;
        }

        private string GetAllowanceBM04(SalaryProcessDto curent, SalaryProcessDto old = null)
        {
            string result = "";
            if(old == null)
            {
                if (!string.IsNullOrEmpty(curent.LeadershipPositionAllowance))
                {
                    result = result + $"Phụ cấp chức vụ lãnh đạo: {curent.LeadershipPositionAllowance}\n";
                }
                if (!string.IsNullOrEmpty(curent.ToxicAllowance))
                {
                    result = result + $"Phụ cấp độc hại: {curent.ToxicAllowance}\n";
                }
                if (!string.IsNullOrEmpty(curent.AreaAllowance))
                {
                    result = result + $"Phụ cấp khu vực: {curent.AreaAllowance}\n";
                }
                if (!string.IsNullOrEmpty(curent.ResponsibilityAllowance))
                {
                    result = result + $"Phụ cấp trách nhiệm: {curent.ResponsibilityAllowance}\n";
                }
                if (!string.IsNullOrEmpty(curent.MobileAllowance))
                {
                    result = result + $"Phụ cấp lưu động: {curent.MobileAllowance}\n";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(curent.LeadershipPositionAllowance) || !string.IsNullOrEmpty(old.LeadershipPositionAllowance))
                {
                    if(curent.LeadershipPositionAllowance != old.LeadershipPositionAllowance)
                    {
                        result = result + $"Phụ cấp chức vụ lãnh đạo mới: {curent.LeadershipPositionAllowance}, Phụ cấp chức vụ lãnh đạo cũ: {old.LeadershipPositionAllowance}\n";
                    }
                    else
                    {
                        result = result + $"Phụ cấp chức vụ lãnh đạo: {curent.LeadershipPositionAllowance}\n";
                    }
                }
                if (!string.IsNullOrEmpty(curent.ToxicAllowance) || !string.IsNullOrEmpty(old.ToxicAllowance))
                {
                    if (curent.ToxicAllowance != old.ToxicAllowance)
                    {
                        result = result + $"Phụ cấp độc hại mới: {curent.ToxicAllowance}, Phụ cấp độc hại cũ: {old.ToxicAllowance}\n";
                    }
                    else
                    {
                        result = result + $"Phụ cấp độc hại: {curent.ToxicAllowance}\n";
                    }
                }
                if (!string.IsNullOrEmpty(curent.AreaAllowance) || !string.IsNullOrEmpty(old.AreaAllowance))
                {
                    if (curent.AreaAllowance != old.AreaAllowance)
                    {
                        result = result + $"Phụ cấp khu vực mới: {curent.AreaAllowance}, Phụ cấp khu vực cũ: {old.AreaAllowance}\n";
                    }
                    else
                    {
                        result = result + $"Phụ cấp khu vực: {curent.AreaAllowance}\n";
                    }
                }
                if (!string.IsNullOrEmpty(curent.ResponsibilityAllowance) || !string.IsNullOrEmpty(old.ResponsibilityAllowance))
                {
                    if (curent.ResponsibilityAllowance != old.ResponsibilityAllowance)
                    {
                        result = result + $"Phụ cấp trách nhiệm mới: {curent.ResponsibilityAllowance}, Phụ cấp trách nhiệm cũ: {old.ResponsibilityAllowance}\n";
                    }
                    else
                    {
                        result = result + $"Phụ cấp trách nhiệm: {curent.ResponsibilityAllowance}\n";
                    }
                }
                if (!string.IsNullOrEmpty(curent.MobileAllowance) || !string.IsNullOrEmpty(old.MobileAllowance))
                {
                    if (curent.MobileAllowance != old.MobileAllowance)
                    {
                        result = result + $"Phụ cấp lưu động mới: {curent.MobileAllowance}, Phụ cấp lưu động cũ: {old.MobileAllowance}\n";
                    }
                    else
                    {
                        result = result + $"Phụ cấp lưu động: {curent.MobileAllowance}\n";
                    }
                }
            }
            return result;
        }

        [AbpAllowAnonymous]
        public async Task<FileDto> ExportHS02(ExportHS02Dto person)
        {
            person.TrainningInfos = await _trainningInfoAppService.GetAll(new TrainningInfos.Dto.GetAllFilter { PersonId = person.Id });
            person.WorkingProcesses = await _workingProcessAppService.GetAll(new WorkingProcesses.Dto.GetAllWorkingProcessFilter { PersonId = person.Id });
            person.RelationSelf = await _relationShipAppService.GetAll(new RelationShips.Dto.GetAllFilter { PersonId = person.Id, Type = StatusEnum.RelationType.Self });
            person.RelationOther = await _relationShipAppService.GetAll(new RelationShips.Dto.GetAllFilter { PersonId = person.Id, Type = StatusEnum.RelationType.Other });
            person.SalaryProcesses = await _salaryProcessAppService.GetAll(person.Id);
            return await _reportExcelExporter.ExportHS02(person);
        }
        private string GetOrgName(int id)
        {
            var result = _organizationUnitAppService.GetOrganizationUnits().Result.Items.FirstOrDefault(x=>x.Id == id).DisplayName;
            return result;
        }
    }
}
