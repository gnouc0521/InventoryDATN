using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.Categories;
using bbk.netcore.mdl.PersonalProfile.Application.CivilServants;
using bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs;
using bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using bbk.netcore.Storage.FileSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles
{
    [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Management)]
    public class PersonalProfileAppService : ApplicationService,  IPersonalProfileAppService
    {
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        private readonly IRepository<ProfileStaff> _profileStaffRepository;
        private readonly ICategoryAppService _categoryAppService;
        private readonly ICivilServantAppService _civilServantAppService;
        private readonly IOrganizationUnitStaffAppService _organizationUnitStaffAppService;
        private readonly IAbpSession _session;


        public PersonalProfileAppService(
            IRepository<ProfileStaff> profileStaffRepository,
            ICategoryAppService categoryAppService,
            ICivilServantAppService civilServantAppService,
            IOrganizationUnitStaffAppService organizationUnitStaffAppService,
            IFileSystemBlobProvider fileSystemBlobProvider,
            IAbpSession session)
        {
            _fileSystemBlobProvider = fileSystemBlobProvider;
            _profileStaffRepository = profileStaffRepository;
            _categoryAppService = categoryAppService;
            _civilServantAppService = civilServantAppService;
            _organizationUnitStaffAppService = organizationUnitStaffAppService;
            _session = session;
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<PagedResultDto<PersonListDto>> GetAll(GetAllPersonInput input)
        {
            var list = (from person in _profileStaffRepository.GetAll().ToList()
                        join category in _categoryAppService.GetListPosition().Result on person.CurrentPosition equals category.Id
                        join civilServant in _civilServantAppService.GetAllCivilServant().Result on person.CivilServantSector equals civilServant.Id
                        join organizationUnit in _organizationUnitStaffAppService.GetAll().Result
                        .WhereIf(input.OrganizationUnitId.HasValue, u => u.OrganizationUnitId == input.OrganizationUnitId) on person.Id equals organizationUnit.StaffId
                        join salaryLevel in _civilServantAppService.GetAllSalaryLevel().Result on person.PayRate equals salaryLevel.Id
                        select new PersonListDto
                        {
                            Id = person.Id,
                            FullName = person.FullName.Trim(),
                            Position = category.Title,
                            Age = GetAge.GetRealAge(person.DateOfBirth),
                            StartedDate = person.RecruitmentDate,
                            CoefficientsSalary = salaryLevel.CoefficientsSalary,
                            CivilServant = civilServant.Name
                        }).WhereIf(!string.IsNullOrEmpty(input.Keyword), u => u.FullName.ToLower().Contains(input.Keyword.ToLower().ToString()))
                        .OrderBy(u => u.FullName.Split(' ').Last())
                        .ToList();
            var result = list.Skip(input.SkipCount).Take(input.MaxResultCount);
            return await Task.FromResult(new PagedResultDto<PersonListDto>(list.Count(), ObjectMapper.Map<List<PersonListDto>>(result)));
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<PagedResultDto<PersonListDto>> GetAllFilterUserId(GetAllPersonInput input)
        {
            var userId = _session.UserId;
            var list = (from person in _profileStaffRepository.GetAll().ToList()
                        join category in _categoryAppService.GetListPosition().Result on person.CurrentPosition equals category.Id
                        join civilServant in _civilServantAppService.GetAllCivilServant().Result on person.CivilServantSector equals civilServant.Id
                        join organizationUnitStaff in _organizationUnitStaffAppService.GetAll().Result
                        .WhereIf(input.OrganizationUnitId.HasValue, u => u.OrganizationUnitId == input.OrganizationUnitId) on person.Id equals organizationUnitStaff.StaffId
                        join organizationUnit in _organizationUnitStaffAppService.GetListOrgByUserId((long)userId).Result on organizationUnitStaff.OrganizationUnitId equals organizationUnit.Id
                        join salaryLevel in _civilServantAppService.GetAllSalaryLevel().Result on person.PayRate equals salaryLevel.Id
                        select new PersonListDto
                        {
                            Id = person.Id,
                            FullName = person.FullName.Trim(),
                            Position = category.Title,
                            Age = GetAge.GetRealAge(person.DateOfBirth),
                            StartedDate = person.RecruitmentDate,
                            CoefficientsSalary = salaryLevel.CoefficientsSalary,
                            CivilServant = civilServant.Name
                        }).WhereIf(!string.IsNullOrEmpty(input.Keyword), u => u.FullName.ToLower().Contains(input.Keyword.Trim().ToLower().ToString()))
                        .OrderBy(u => u.FullName.Split(' ').Last())
                        .ThenBy(u => u.FullName.Split(' ').Count() >= 1 ? u.FullName.Split(' ')[0] : "")
                        .ThenBy(u => u.FullName.Split(' ').Count() >= 3 ? u.FullName.Split(' ')[1] : "")
                        .ThenBy(u => u.FullName.Split(' ').Count() >= 4 ? u.FullName.Split(' ')[2] : "")
                        .ToList();
            var result = list.Skip(input.SkipCount).Take(input.MaxResultCount);
            return await Task.FromResult(new PagedResultDto<PersonListDto>(list.Count(), ObjectMapper.Map<List<PersonListDto>>(result)));
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<PersonalProfileDto>> GetAllList()
        {
            var list = await _profileStaffRepository.GetAllListAsync();
            var result = list.OrderBy(u => u.FullName.Trim().Split(' ').Last())
                        .ThenBy(u => u.FullName.Trim().Split(' ').Count() >= 1 ? u.FullName.Trim().Split(' ')[0] : "")
                        .ThenBy(u => u.FullName.Trim().Split(' ').Count() >= 3 ? u.FullName.Trim().Split(' ')[1] : "")
                        .ThenBy(u => u.FullName.Trim().Split(' ').Count() >= 4 ? u.FullName.Trim().Split(' ')[2] : "")
                        .ToList();
            return ObjectMapper.Map<List<PersonalProfileDto>>(result);
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<PersonalProfileDto> Get(int id)
        {
            try
            {
                var userId = _session.UserId;
                var checkPermission = await _organizationUnitStaffAppService.CheckPermissionStaffView((long)userId, id);
                if (checkPermission == false)
                    return null;
                var obj = await _profileStaffRepository.GetAsync(id);
                var person = ObjectMapper.Map<PersonalProfileDto>(obj);
                var organizationUnitStaff = await _organizationUnitStaffAppService.GetByStaffId(id);
                if(organizationUnitStaff != null)
                {
                    person.OrganizationUnitId = organizationUnitStaff.OrganizationUnitId;
                }
                return person;
            }
            catch
            {
                return null;
            }
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View, PersonalProfilePermission.Pages_PPS_Staff_Create)]
        public async Task<int> Create(CreatePersonalProfileDto input)
        {
            try
            {
                var isExist = await _profileStaffRepository.GetAll().AnyAsync(u => u.FullName == input.FullName && u.IdentityCardNo == input.IdentityCardNo);
                if (isExist)
                    throw new UserFriendlyException("Thông tin cán bộ đã tồn tại");
                Type type = input.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                foreach (PropertyInfo property in props)
                {
                    var propName = property.Name;
                    if (property.PropertyType == typeof(string) && propName != "HighestArmyRank" && propName != "SonOfFamilyPolicy"
                        && propName != "DateOfJoiningSocialPoliticalAssosiations" && propName != "Note" && propName != "NativePlaceDescription"
                         && propName != "Height" && propName != "Weight" && propName != "BloodGroup" && propName != "HealthStatus")
                    {
                        object value = property.GetValue(input, null);
                        if (value == null || value.ToString() == "")
                        {
                            property.SetValue(input, "Không");
                        }
                    }
                }
                var obj = ObjectMapper.Map<ProfileStaff>(input);
                int id = await _profileStaffRepository.InsertAndGetIdAsync(obj);
                if (input.OrganizationUnitId.HasValue)
                {
                    var ouInpt = new OrganizationUnitStaffDto
                    {
                        StaffId = id,
                        OrganizationUnitId = (long)input.OrganizationUnitId
                    };
                    await _organizationUnitStaffAppService.Create(ouInpt);
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<EditPersonalProfileDto> Update(EditPersonalProfileDto input)
        {
            try
            {
                var isExist = await _profileStaffRepository.GetAll().AnyAsync(u => u.FullName == input.FullName && u.IdentityCardNo == input.IdentityCardNo && u.Id != input.Id);
                if (isExist)
                    throw new UserFriendlyException("Thông tin cán bộ đã tồn tại");

                Type type = input.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                foreach (PropertyInfo property in props)
                {
                    var propName = property.Name;
                    if (property.PropertyType == typeof(string) && propName != "HighestArmyRank" && propName != "SonOfFamilyPolicy"
                        && propName != "DateOfJoiningSocialPoliticalAssosiations" && propName != "Note" && propName != "NativePlaceDescription"
                        && propName != "Height" && propName != "Weight" && propName != "BloodGroup" && propName != "HealthStatus")
                    {
                        object value = property.GetValue(input, null);
                        if (value == null || value.ToString() == "")
                        {
                            property.SetValue(input, "Không");
                        }
                    }
                }

                var obj = await _profileStaffRepository.GetAsync(input.Id);
                var data = ObjectMapper.Map<EditPersonalProfileDto, ProfileStaff>(input, obj);
                if (input.OrganizationUnitId.HasValue)
                {
                    var ouStaff = await _organizationUnitStaffAppService.GetByStaffId(input.Id);
                    var inputOUS = new OrganizationUnitStaffDto
                    {
                        StaffId = input.Id,
                        OrganizationUnitId = (long)input.OrganizationUnitId
                    };
                    if (ouStaff == null)
                    {
                        await _organizationUnitStaffAppService.Create(inputOUS);
                    }
                    else
                    {
                        inputOUS.Id = ouStaff.Id;
                        await _organizationUnitStaffAppService.Update(inputOUS);
                    }
                }
                await _profileStaffRepository.UpdateAsync(data);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            return input;
        }

        public async Task<Address> GetAddress(string filePath, string superiorId)
        {
            string file = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.DataAddress + @"\\" + filePath));
            var data = ReadJson<Address>.ConvertJsonToObject(file);
            Address result = new Address
            {
                Addresses = data.Addresses.WhereIf(!string.IsNullOrEmpty(superiorId), u => u.SuperiorId == superiorId).OrderBy(u => u.Name).ToList()
            };
            return await Task.FromResult(result);
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task UpdateImage(string imageURL, string imagePath, int id)
        {
            try
            {
                var person = _profileStaffRepository.Get(id);
                string delete_image="";
                if (person != null)
                {
                    delete_image = person.ImagePath;
                    person.ImagePath = imagePath;
                    person.ImageURL = imageURL;
                    await _profileStaffRepository.UpdateAsync(person);
                    await _fileSystemBlobProvider.DeleteAsync(delete_image);
                }
                
                
                
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
