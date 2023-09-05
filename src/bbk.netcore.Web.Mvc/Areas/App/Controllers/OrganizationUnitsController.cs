using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Organizations;
using bbk.netcore.Authorization;
using bbk.netcore.Controllers;
using bbk.netcore.Web.Areas.App.Models.Common.Modals;
using bbk.netcore.Web.Areas.App.Models.OrganizationUnits;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.App.Controllers
{
    [Area("App")]
    //[AbpMvcAuthorize(PermissionNames.Pages_Administration_OrganizationUnits)]
    public class OrganizationUnitsController : netcoreControllerBase
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        public OrganizationUnitsController(IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            _organizationUnitRepository = organizationUnitRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        //[AbpMvcAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public PartialViewResult CreateModal(long? parentId)
        {
            return PartialView("_CreateModal", new CreateOrganizationUnitModalViewModel(parentId));
        }

        //[AbpMvcAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<PartialViewResult> EditModal(long id)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(id);
            var model = ObjectMapper.Map<EditOrganizationUnitModalViewModel>(organizationUnit);

            return PartialView("_EditModal", model);
        }

        //[AbpMvcAuthorize(PermissionNames.Pages_Administration_OrganizationUnits_ManageMembers)]
        public PartialViewResult AddMemberModal(LookupModalViewModel model)
        {
            return PartialView("_AddMemberModal", model);
        }
    }
}
