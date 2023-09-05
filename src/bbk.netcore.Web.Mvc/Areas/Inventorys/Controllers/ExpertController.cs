using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Experts;
using bbk.netcore.mdl.OMS.Application.Itemses;
using bbk.netcore.Web.Areas.Inventorys.Models.Expert;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ExpertController : netcoreControllerBase
    {
        private readonly IItemsService _itemAppService;
        private readonly IExpertAppService _expertAppService;

        public ExpertController(IItemsService itemAppService, IExpertAppService expertAppService)
        {
            _itemAppService = itemAppService;
            _expertAppService = expertAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateExpert()
        {
            var itemList = await _itemAppService.GetItemList();

            ExpertModal modal = new ExpertModal
            {
                itemlist = itemList,
            };
            return PartialView("_CreateModal",modal);
        }

        public async Task<IActionResult> EditExpectModal(int Id)
        {
            var dto = await _expertAppService.GetAsync(new EntityDto(Id));
            return PartialView("_EditModal", dto);
        }

        public async Task<IActionResult> ViewDetail(int Id)
        {
            var dto = await _expertAppService.GetAsync(new EntityDto(Id));
            return View("ViewDetailExpert", dto);
        }

        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }
    }
}
