using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.ProfileWorks;
using bbk.netcore.mdl.OMS.Application.Works;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using bbk.netcore.Web.Areas.Inventorys.Models.WorkGroups;
using bbk.netcore.Web.Areas.Inventorys.Models.ProfileWork;
using Abp.Web.Models;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ProfileWorkController : netcoreControllerBase
    {
        private readonly IProfileWorkAppService _profileWorkAppService;
        private readonly UserManager _userManager;
        public ProfileWorkController(IProfileWorkAppService profileWorkAppService, UserManager userManager)
        {
            _profileWorkAppService = profileWorkAppService;
            _userManager = userManager;
        }
        public async Task<ActionResult> Index()
        {
            var listWorkGroup = _profileWorkAppService.GetAllList();
            IndexViewProfileWorkModel model = new IndexViewProfileWorkModel()
            {
                profileWorkGroupListDtos = listWorkGroup.Result.Items,
            };
            return View(model);
        }

        public PartialViewResult CreateProfileWork()
        {
            return PartialView("_CreateModal");
        }
        public async Task<IActionResult> CreateProfileWorkItems(int Id)
        {
            var dto = await _profileWorkAppService.GetAsync(new EntityDto(Id));
            return PartialView("_CreateWorkItemsModal", dto.Items[0]);
        }
        public async Task<IActionResult> EditProfileWork(int Id)
        {
            var Dto = await _profileWorkAppService.GetAsync(new EntityDto(Id));
            return PartialView("_EditModal", Dto.Items[0]);
        }

        //Code Hà

        [HttpPost]
        public async Task<JsonResult> View(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }
        public async Task<ActionResult> ViewDetails(int id)
        {
            var Dto = await _profileWorkAppService.GetAsync(new EntityDto(id));
            return View("ViewDetails", Dto.Items[0]);
        }
    }
}
