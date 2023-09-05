using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs.Dto;
using bbk.netcore.Organizations.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs
{
    public interface IOrganizationUnitStaffAppService : IApplicationService
    {
        Task<List<OrganizationUnitStaffDto>> GetAll();

        Task<List<OrganizationUnitDto>> GetListOrgByUserId(long userId);

        Task<bool> CheckPermissionStaffView(long userId, int staffId);

        Task<OrganizationUnitStaffDto> GetByStaffId(int staffId);

        Task<OrganizationUnitStaffDto> Create(OrganizationUnitStaffDto input);

        Task<OrganizationUnitStaffDto> Update(OrganizationUnitStaffDto input);
    }
}
