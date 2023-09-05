using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.Mvc.Controllers;
using bbk.netcore.mdl.OMS.Application.Units;
using bbk.netcore.mdl.OMS.Application.WarehouseTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class WarehouseTypesController : AbpController
    {
        private readonly IWarehouseTypesService _warehouseTypesService;

        public WarehouseTypesController(
            IWarehouseTypesService warehouseTypesService
            )
        {
            _warehouseTypesService = warehouseTypesService;
        }

        public async Task<ActionResult> Index()
        {

            return View();
        }
        public PartialViewResult CreateWarehouseType()
        {
            return PartialView("_CreateModal");
        }
        public async Task<IActionResult> EditWarehouseTypeModal(int Id)
        {
            var dto = await _warehouseTypesService.GetAsync(new EntityDto(Id));
            return PartialView("_EditModal", dto);
        }
    }
}
