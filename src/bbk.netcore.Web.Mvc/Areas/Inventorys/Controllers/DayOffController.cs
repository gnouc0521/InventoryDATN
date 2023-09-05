using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.DayOffs;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class DayOffController : netcoreControllerBase
    {
        private readonly IDayOffAppService _dayOffAppService;
        private readonly UserManager _userManager;

        public DayOffController(IDayOffAppService dayOffAppService, UserManager userManager)
        {
            _dayOffAppService = dayOffAppService;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult CreateDayOff()
        {
            return PartialView("_CreateModal");
        }

        public async Task<IActionResult> EditDayOffModal(int Id)
        {
            var dto = await _dayOffAppService.GetAsync(new EntityDto(Id));
            return PartialView("_EditModal", dto);
        }
    }
}
