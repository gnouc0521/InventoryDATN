using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises;
using bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.Web.Areas.Inventorys.Models.PurchsesRequest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class PurchaseAssignmentController : netcoreControllerBase
    {
        private readonly IPurchasesRequestsService _purchasesRequestsService;
        private readonly IPurchasesSynthesiseAppService _purchasesSynthesiseAppService;
      
      
        public PurchaseAssignmentController(
            IPurchasesRequestsService purchasesRequestsService ,
            IPurchasesSynthesiseAppService purchasesSynthesiseAppService)
        {
            _purchasesRequestsService = purchasesRequestsService;
            _purchasesSynthesiseAppService = purchasesSynthesiseAppService;
           
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> ViewPurchaseAssignment(long Id )
        {
            var model = await _purchasesSynthesiseAppService.GetAsync(new EntityDto<long>(Id));
            ViewBag.SynthesiseId = Id;
           
            return PartialView("ViewPurchaseAssignment", model);
        }

        public async Task<ActionResult> DetailPurchaseAssignment(long Id)
        {
            var model = await _purchasesSynthesiseAppService.GetAsync(new EntityDto<long>(Id));
            ViewBag.SynthesiseId = Id;
           
            return View("DetailPurchaseAssignment",model);
        }

        public async Task<ActionResult> Detail(long Id)
        {
            var model = await _purchasesSynthesiseAppService.GetAsync(new EntityDto<long>(Id));
            ViewBag.SynthesiseId = Id;

            return View("Detail", model);
        }


        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }
    }
}
