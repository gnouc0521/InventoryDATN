using Abp.Application.Services;
using Abp.Domain.Repositories;
using bbk.netcore.mdl.PersonalProfile.Application.Fluctuations.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using Abp.Timing;
using System;
using System.Collections.Generic;
using bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses;
using bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles;
using System.Threading.Tasks;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using System.Linq;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;
using bbk.netcore.Organizations;
using bbk.netcore.mdl.PersonalProfile.Application.Categories;
using bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs;
using Abp.UI;
using Abp.Authorization;
using bbk.netcore.Authorization;

namespace bbk.netcore.mdl.PersonalProfile.Application.Fluctuations
{
    [AbpAuthorize]
    public class FluctuationAppService : ApplicationService, IFluctuationAppService
    {
        private readonly IWorkingProcessAppService _workingProcessAppService;
        private readonly ISalaryProcessAppService _salaryProcessAppService;
        private readonly IPersonalProfileAppService _personalProfileAppService;
        private readonly IRepository<ProfileStaff> _staffRepository;
        private readonly IOrganizationUnitAppService _organizationUnitAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IOrganizationUnitStaffAppService _organizationUnitStaffAppService;

        public FluctuationAppService(
            IWorkingProcessAppService workingProcessAppService,
            IRepository<ProfileStaff> staffRepository, 
            ISalaryProcessAppService salaryProcessAppService,
            IPersonalProfileAppService personalProfileAppService,
            IOrganizationUnitAppService organizationUnitAppService,
            ICategoryAppService categoryAppService,
            IOrganizationUnitStaffAppService organizationUnitStaffAppService)
        {
            _workingProcessAppService = workingProcessAppService;
            _staffRepository = staffRepository;
            _salaryProcessAppService = salaryProcessAppService;
            _personalProfileAppService = personalProfileAppService;
            _organizationUnitAppService = organizationUnitAppService;
            _categoryAppService = categoryAppService;
            _organizationUnitStaffAppService = organizationUnitStaffAppService;
        }
        public async Task<List<FluctuationDto>> GetContractExpiration()
        {
            try
            {
                List<FluctuationDto> lst = new List<FluctuationDto>();
                var lstStaffId = _workingProcessAppService.GetAllListStaffId();

                foreach (var staffId in lstStaffId)
                {
                    var wps = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = staffId });
                    wps = wps.Where(x => x.ToDate.HasValue && x.TypeOfChange.Trim().ToLower().Equals("Ký hợp đồng lao động".Trim().ToLower())).OrderByDescending(x => x.ToDate).ToList();
                    if (wps.Count > 0)
                    {
                        var wp = wps.First();
                        if ((wp.ToDate.Value - Clock.Now).TotalDays > 0 && (wp.ToDate.Value - Clock.Now).TotalDays <= 60)
                        {
                            FluctuationDto fluctuationDto = new FluctuationDto();
                            var staff = _personalProfileAppService.Get(staffId).Result;
                            fluctuationDto.WorkingTitle = _categoryAppService.GetCategorybyId(staff.CurrentPosition).Result.Title;
                            fluctuationDto.Organ = _organizationUnitAppService.GetOrganizationUnits().Result.Items.FirstOrDefault(x => x.Id == _organizationUnitStaffAppService.GetByStaffId(staff.Id).Result.OrganizationUnitId).DisplayName;

                            fluctuationDto.FullName = _staffRepository.Get(wp.PersonId).FullName;
                            fluctuationDto.DecisionNumber = wp.DecisionNumber;
                            fluctuationDto.ToDate = wp.ToDate.Value;
                            fluctuationDto.FluctuationType = FluctuationEnum.ContractExpiration;
                            fluctuationDto.FluctuationTypeString = EnumExtensions.GetDisplayName(fluctuationDto.FluctuationType);
                            lst.Add(fluctuationDto);
                        }
                    }

                }
                return lst;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task<List<FluctuationDto>> GetReappointed()
        {
            try
            {
                List<FluctuationDto> lst = new List<FluctuationDto>();
                var lstStaffId = _workingProcessAppService.GetAllListStaffId();

                foreach (var staffId in lstStaffId)
                {
                    var wps = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = staffId });
                    wps = wps.Where(
                            x => x.ToDate.HasValue &&
                            (x.TypeOfChange.Trim().ToLower().Equals("Bổ nhiệm".Trim().ToLower())
                            || x.TypeOfChange.Trim().ToLower().Equals("Bổ nhiệm lại".Trim().ToLower()))
                            ).OrderByDescending(x => x.ToDate).ToList();
                    if (wps.Count > 0)
                    {
                        var wp = wps.First();
                        if ((wp.ToDate.Value - Clock.Now).TotalDays > 0 && (wp.ToDate.Value - Clock.Now).TotalDays <= 155)
                        {
                            FluctuationDto fluctuationDto = new FluctuationDto();

                            var staff = _personalProfileAppService.Get(staffId).Result;
                            fluctuationDto.WorkingTitle = _categoryAppService.GetCategorybyId(staff.CurrentPosition).Result.Title;
                            fluctuationDto.Organ = _organizationUnitAppService.GetOrganizationUnits().Result.Items.FirstOrDefault(x => x.Id == _organizationUnitStaffAppService.GetByStaffId(staff.Id).Result.OrganizationUnitId).DisplayName;

                            fluctuationDto.FullName = _staffRepository.Get(wp.PersonId).FullName;
                            fluctuationDto.DecisionNumber = wp.DecisionNumber;
                            fluctuationDto.ToDate = wp.ToDate.Value;
                            fluctuationDto.FluctuationType = FluctuationEnum.Reappointed;
                            fluctuationDto.FluctuationTypeString = EnumExtensions.GetDisplayName(fluctuationDto.FluctuationType);
                            lst.Add(fluctuationDto);
                        }
                    }

                }
                return lst;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            
        }

        public async Task<List<FluctuationDto>> GetSalaryIncrease()
        {
            try
            {
                List<FluctuationDto> lst = new List<FluctuationDto>();
                var lstStaffId = _salaryProcessAppService.GetAllListStaffId();

                foreach (var staffId in lstStaffId)
                {
                    var sps = await _salaryProcessAppService.GetAll(staffId);
                    sps = sps.OrderByDescending(x => x.SalaryIncreaseTime).ToList();
                    if (sps.Count > 0)
                    {
                        var sp = sps.First();
                        var payrates = sp.PayRate.Split('/')[1];
                        switch (payrates)
                        {
                            //((date1.Year - date2.Year) * 12) + date1.Month - date2.Month

                            case "6":
                                {
                                    if (((Clock.Now.Year - sp.SalaryIncreaseTime.Year) * 12 + (Clock.Now.Month - sp.SalaryIncreaseTime.Month)) > 58)
                                    {
                                        FluctuationDto fluctuationDto = new FluctuationDto();

                                        var staff = _personalProfileAppService.Get(staffId).Result;
                                        fluctuationDto.WorkingTitle = _categoryAppService.GetCategorybyId(staff.CurrentPosition).Result.Title;
                                        fluctuationDto.Organ = _organizationUnitAppService.GetOrganizationUnits().Result.Items.FirstOrDefault(x => x.Id == _organizationUnitStaffAppService.GetByStaffId(staff.Id).Result.OrganizationUnitId).DisplayName;
                                        
                                        fluctuationDto.FullName = _staffRepository.Get(sp.PersonId).FullName;
                                        fluctuationDto.ToDate = sp.SalaryIncreaseTime.AddMonths(60);
                                        fluctuationDto.DecisionNumber = sp.DecisionNumber;

                                        fluctuationDto.FluctuationType = FluctuationEnum.SalaryIncrease;
                                        fluctuationDto.FluctuationTypeString = EnumExtensions.GetDisplayName(fluctuationDto.FluctuationType);
                                        lst.Add(fluctuationDto);
                                    }
                                    break;
                                }
                            case "9":
                            case "10":
                                {
                                    if (((Clock.Now.Year - sp.SalaryIncreaseTime.Year) * 12 + (Clock.Now.Month - sp.SalaryIncreaseTime.Month)) > 34)
                                    {
                                        FluctuationDto fluctuationDto = new FluctuationDto();

                                        var staff = _personalProfileAppService.Get(staffId).Result;
                                        fluctuationDto.WorkingTitle = _categoryAppService.GetCategorybyId(staff.CurrentPosition).Result.Title;
                                        fluctuationDto.Organ = _organizationUnitAppService.GetOrganizationUnits().Result.Items.FirstOrDefault(x => x.Id == _organizationUnitStaffAppService.GetByStaffId(staff.Id).Result.OrganizationUnitId).DisplayName;

                                        fluctuationDto.FullName = _staffRepository.Get(sp.PersonId).FullName;
                                        fluctuationDto.ToDate = sp.SalaryIncreaseTime.AddMonths(36);
                                        fluctuationDto.DecisionNumber = sp.DecisionNumber;

                                        //var wps = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = staffId });
                                        //wps = wps.Where(x => (
                                        //x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                                        //    && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                                        //    && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower())
                                        //)).OrderByDescending(x => x.IssuedDate).ToList();
                                        //if (wps.Count > 0)
                                        //{
                                        //    var wp = wps.First();
                                        //    fluctuationDto.WorkingTitle = wp.WorkingTitle;
                                        //    fluctuationDto.Organ = wp.OrganName;
                                        //}

                                        fluctuationDto.FluctuationType = FluctuationEnum.SalaryIncrease;
                                        fluctuationDto.FluctuationTypeString = EnumExtensions.GetDisplayName(fluctuationDto.FluctuationType);
                                        lst.Add(fluctuationDto);
                                    }
                                    break;
                                }
                            case "12":
                                {
                                    if (((Clock.Now.Year - sp.SalaryIncreaseTime.Year) * 12 + (Clock.Now.Month - sp.SalaryIncreaseTime.Month)) > 22)
                                    {
                                        FluctuationDto fluctuationDto = new FluctuationDto();

                                        var staff = _personalProfileAppService.Get(staffId).Result;
                                        fluctuationDto.WorkingTitle = _categoryAppService.GetCategorybyId(staff.CurrentPosition).Result.Title;
                                        fluctuationDto.Organ = _organizationUnitAppService.GetOrganizationUnits().Result.Items.FirstOrDefault(x => x.Id == _organizationUnitStaffAppService.GetByStaffId(staff.Id).Result.OrganizationUnitId).DisplayName;

                                        fluctuationDto.FullName = _staffRepository.Get(sp.PersonId).FullName;
                                        fluctuationDto.ToDate = sp.SalaryIncreaseTime.AddMonths(24);
                                        fluctuationDto.DecisionNumber = sp.DecisionNumber;

                                        //var wps = await _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = staffId });
                                        //wps = wps.Where(x => (x.TypeOfChange.Trim().ToLower() != ("Nghỉ hưu".Trim().ToLower())
                                        //    && x.TypeOfChange.Trim().ToLower() != ("Thôi việc".Trim().ToLower())
                                        //    && x.TypeOfChange.Trim().ToLower() != ("Chấm dứt hợp đồng lao động".Trim().ToLower()))).OrderByDescending(x => x.IssuedDate).ToList();
                                        //if (wps.Count > 0)
                                        //{
                                        //    var wp = wps.First();
                                        //    fluctuationDto.WorkingTitle = wp.WorkingTitle;
                                        //    fluctuationDto.Organ = wp.OrganName;
                                        //}
                                        fluctuationDto.FluctuationType = FluctuationEnum.SalaryIncrease;
                                        fluctuationDto.FluctuationTypeString = EnumExtensions.GetDisplayName(fluctuationDto.FluctuationType);
                                        lst.Add(fluctuationDto);
                                    }
                                    break;
                                }
                            default:
                                break;

                        }
                    }

                }
                return lst;
            }
            catch (Exception e)
            {

                throw new UserFriendlyException(e.Message);
            }
            
        }

        public async Task<List<FluctuationDto>> GetRetirement()
        {
            try
            {
                List<FluctuationDto> lst = new List<FluctuationDto>();
                var lstStaff = await _personalProfileAppService.GetAllList();

                foreach (var staff in lstStaff)
                {
                    DateTime? retireTime = CheckRetirement.CheckRetire(staff.Gender, staff.DateOfBirth);
                    if (retireTime != null && retireTime.HasValue)
                    {
                        FluctuationDto fluctuationDto = new FluctuationDto();
                        fluctuationDto.WorkingTitle = _categoryAppService.GetCategorybyId(staff.CurrentPosition).Result.Title;
                        fluctuationDto.Organ = _organizationUnitAppService.GetOrganizationUnits().Result.Items.FirstOrDefault(x => x.Id == _organizationUnitStaffAppService.GetByStaffId(staff.Id).Result.OrganizationUnitId).DisplayName;
                        fluctuationDto.FullName = staff.FullName;
                        fluctuationDto.ToDate = retireTime.Value;
                        var wp = _workingProcessAppService.GetAll(new GetAllWorkingProcessFilter() { PersonId = staff.Id })
                                    .Result.Where(x => (x.TypeOfChange.Trim().ToLower().Equals("Nghỉ hưu".Trim().ToLower()))).FirstOrDefault();
                        if (wp != null)
                        {
                            fluctuationDto.DecisionNumber = wp.DecisionNumber;
                            fluctuationDto.FluctuationType = FluctuationEnum.Retirement;
                            fluctuationDto.FluctuationTypeString = EnumExtensions.GetDisplayName(fluctuationDto.FluctuationType);
                        }
                        lst.Add(fluctuationDto);
                    }
                }
                return lst;
            }
            catch (Exception e)
            {

                throw new UserFriendlyException(e.Message);
            }
            
        }
    }
}
