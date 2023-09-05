using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using bbk.netcore.Authorization;
using bbk.netcore.Authorization.Permissions;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Roles.Dto;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Security;
using bbk.netcore.Web.Areas.App.Models.Users;
using bbk.netcore.Web.Controllers;
using bbk.netcore.Controllers;
using System;

namespace bbk.netcore.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(PermissionNames.Pages_Administration_Users)]
    public class UsersController : netcoreControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;
        private readonly IUserLoginAppService _userLoginAppService;
        private readonly IRoleAppService _roleAppService;
        private readonly IPermissionAppService _permissionAppService;
        private readonly IPasswordComplexitySettingStore _passwordComplexitySettingStore;

        public UsersController(
            IUserAppService userAppService,
            UserManager userManager,
            IUserLoginAppService userLoginAppService,
            IRoleAppService roleAppService,
            IPermissionAppService permissionAppService,
            IPasswordComplexitySettingStore passwordComplexitySettingStore)
        {
            _userAppService = userAppService;
            _userManager = userManager;
            _userLoginAppService = userLoginAppService;
            _roleAppService = roleAppService;
            _permissionAppService = permissionAppService;
            _passwordComplexitySettingStore = passwordComplexitySettingStore;
        }

        public async Task<ActionResult> Index()
        {
            var roles = new List<ComboboxItemDto>();
            var permissions = _permissionAppService.GetAllPermissions()
                                                    .Items
                                                    .Select(p => new ComboboxItemDto(p.Name, new string('-', p.Level * 2) + " " + p.DisplayName))
                                                    .ToList();

            if (IsGranted(PermissionNames.Pages_Administration_Roles))
            {
                var getRolesOutput = await _roleAppService.GetRoles(new GetRolesInput());
                roles = getRolesOutput.Items.Select(r => new ComboboxItemDto(r.Id.ToString(), r.DisplayName)).ToList();
            }

            roles.Insert(0, new ComboboxItemDto("", ""));
            permissions.Insert(0, new ComboboxItemDto("", ""));

            var model = new UsersViewModel
            {
                FilterText = Request.Query["filterText"],
                Roles = roles,
                Permissions = permissions,
                OnlyLockedUsers = false
            };

            return View(model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_Users_Create, PermissionNames.Pages_Administration_Users_Edit)]
        public async Task<ActionResult> CreateOrEditModal(long? id)
        {
            try
            {
                var output = await _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                var viewModel = new CreateOrEditUserModalViewModel(output)
                {
                    PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync()
                };

                return PartialView("_CreateOrEditModal", viewModel);
            }
            catch
            {
                return PartialView("_CreateOrEditModal");
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Administration_Users_ChangePermissions)]
        public async Task<PartialViewResult> PermissionsModal(long id)
        {
            var user = await _userManager.GetUserByIdAsync(id);
            var output = await _userAppService.GetUserPermissionsForEdit(new EntityDto<long>(id));
            var viewModel = new UserPermissionsEditViewModel(output, user);

            return PartialView("_PermissionsModal", viewModel);
        }

        public async Task<PartialViewResult> LoginAttemptsModal()
        {
            var output = await _userLoginAppService.GetRecentUserLoginAttempts();
            var model = new UserLoginAttemptModalViewModel
            {
                LoginAttempts = output.Items.ToList()
            };
            return PartialView("_LoginAttemptsModal", model);
        }
    }
}