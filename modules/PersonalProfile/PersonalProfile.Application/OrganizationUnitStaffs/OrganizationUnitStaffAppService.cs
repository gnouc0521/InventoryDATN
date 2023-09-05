using Abp.Application.Services;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.Organizations;
using bbk.netcore.Organizations.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs
{
    [AbpAuthorize]
    public class OrganizationUnitStaffAppService: ApplicationService, IOrganizationUnitStaffAppService
    {
        private readonly IRepository<OrganizationUnitStaff, long> _organizationUnitStaffRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IOrganizationUnitAppService _organizationUnitAppService;

        public OrganizationUnitStaffAppService(
            IRepository<OrganizationUnitStaff, long> organizationUnitStaffRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitAppService organizationUnitAppService)
        {
            _organizationUnitStaffRepository = organizationUnitStaffRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitAppService = organizationUnitAppService;
        }

        public async Task<List<OrganizationUnitStaffDto>> GetAll()
        {
            try
            {
                var query = await _organizationUnitStaffRepository.GetAll()
                    .OrderByDescending(u => u.CreationTime).ToListAsync();
                return ObjectMapper.Map<List<OrganizationUnitStaffDto>>(query);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<List<OrganizationUnitDto>> GetListOrgByUserId(long userId)
        {
            try
            {
                var orgs = await _organizationUnitAppService.GetOrganizationUnits();
                var query = (from org in orgs.Items
                             join orgUser in _userOrganizationUnitRepository.GetAll() on org.Id equals orgUser.OrganizationUnitId
                             where orgUser.UserId == userId
                             select new OrganizationUnitDto
                             {
                                 Id = org.Id,
                                 DisplayName = org.DisplayName,
                                 Code = org.Code,
                                 ParentId = org.ParentId,
                                 MemberCount = org.MemberCount,
                                 CreationTime = org.CreationTime,
                                 CreatorUserId = org.CreatorUserId,
                                 LastModificationTime = org.LastModificationTime,
                                 LastModifierUserId = org.LastModifierUserId
                             }).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<bool> CheckPermissionStaffView(long userId, int staffId)
        {
            try
            {
                var orgUsers = await _userOrganizationUnitRepository.GetAll().Where(u => u.UserId == userId).ToListAsync();
                var orgStaff = await GetByStaffId(staffId);
                var result = orgUsers.Any(u => u.OrganizationUnitId == orgStaff.OrganizationUnitId);
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<OrganizationUnitStaffDto> GetByStaffId(int staffId)
        {
            try
            {
                var query = await _organizationUnitStaffRepository.GetAll().Where(u => u.StaffId == staffId)
                    .OrderByDescending(u => u.CreationTime).FirstOrDefaultAsync();
                return ObjectMapper.Map<OrganizationUnitStaffDto>(query);
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<OrganizationUnitStaffDto> Create(OrganizationUnitStaffDto input)
        {
            try
            {
                var obj = ObjectMapper.Map<OrganizationUnitStaff>(input);
                input.Id = await _organizationUnitStaffRepository.InsertAndGetIdAsync(obj);
                return input;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<OrganizationUnitStaffDto> Update(OrganizationUnitStaffDto input)
        {
            try
            {
                var data = await _organizationUnitStaffRepository.GetAsync(input.Id);
                var obj = ObjectMapper.Map<OrganizationUnitStaffDto, OrganizationUnitStaff>(input, data);
                await _organizationUnitStaffRepository.UpdateAsync(obj);
                return input;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
