using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Ruleses;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{

    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class RulesController : netcoreControllerBase
    {
        private readonly IRulesService _rulesService;
        public RulesController(IRulesService rulesService)
        {
            _rulesService = rulesService;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }
        public PartialViewResult CreateRules()
        {
            return PartialView("_CreateModal");
        }
        public async Task<IActionResult> EditRulesModal(int Id)
        {
            var dto = await _rulesService.GetAsync(new EntityDto(Id));
            return PartialView("_EditModal", dto);
        }

    }
}
