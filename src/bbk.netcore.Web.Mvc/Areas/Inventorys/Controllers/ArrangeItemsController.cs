using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.WarehouseLocationItem;
using Microsoft.AspNetCore.Mvc;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ArrangeItemsController : netcoreControllerBase
    {
        private readonly IWarehouseLocationItemService _warehouseLocationItemService;
        public ArrangeItemsController(IWarehouseLocationItemService warehouseLocationItemService)
        {
            _warehouseLocationItemService = warehouseLocationItemService;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
