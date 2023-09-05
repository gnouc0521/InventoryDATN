using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.ScheduleWorks;
using bbk.netcore.mdl.OMS.Core.Enums;
using bbk.netcore.Web.Areas.Inventorys.Models.ScheduleWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ScheduleWorkController : netcoreControllerBase
    {
        private readonly IScheduleWorkAppService _scheduleWorkAppService;
        public ScheduleWorkController(IScheduleWorkAppService scheduleWorkAppService)
        {
            _scheduleWorkAppService = scheduleWorkAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult CreateScheduleWork()
        {
            return PartialView("_CreateScheduleWorkModal");
        }

        public async Task<IActionResult> EditScheduleWorkModal(int Id)
        {
            var dto = await _scheduleWorkAppService.GetAsync(new EntityDto(Id));
            var modal = new IndexViewScheduleWorkModel()
            {
                scheduleWorkList = dto,

            };
            return PartialView("_EditScheduleWorkModal", modal);
        }
    }
}
