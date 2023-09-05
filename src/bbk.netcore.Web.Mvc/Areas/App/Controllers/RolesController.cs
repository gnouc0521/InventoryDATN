using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Authorization;
using bbk.netcore.Authorization.Permissions;
using bbk.netcore.Controllers;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Web.Areas.App.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RolesController : netcoreControllerBase
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IPermissionAppService _permissionAppService;

        public RolesController(
            IRoleAppService roleAppService,
            IPermissionAppService permissionAppService)
        {
            _roleAppService = roleAppService;
            _permissionAppService = permissionAppService;
        }

        public ActionResult Index()
        {
            var permissions = _permissionAppService.GetAllPermissions()
                                                .Items
                                                .Select(p => new ComboboxItemDto(p.Name, new string('-', p.Level * 2) + " " + p.DisplayName))
                                                .ToList();

            permissions.Insert(0, new ComboboxItemDto("", ""));
            var model = new RoleListViewModel
            {
                Permissions = permissions
            };

            return View(model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_Roles_Create, PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            var output = await _roleAppService.GetRoleForEdit(new NullableIdDto { Id = id });
            var viewModel = new CreateOrEditRoleModalViewModel(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}
