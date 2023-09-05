using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.ImportRequests;
using bbk.netcore.mdl.OMS.Application.InventoryTickets;
using bbk.netcore.mdl.OMS.Application.Itemses;
using bbk.netcore.mdl.OMS.Application.Ruleses;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Application.Units;
using bbk.netcore.mdl.OMS.Application.WareHouses;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.Web.Areas.Inventorys.Models.InventoryTicket;
using bbk.netcore.Web.Areas.Inventorys.Models.Stock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    public class StockController : netcoreControllerBase
    {
        private readonly IWareHouseAppService _wareHouseAppService;
        private readonly IRulesService _rulesService;

        public StockController(IWareHouseAppService wareHouseAppService, IRulesService rulesService)
        {
            _wareHouseAppService = wareHouseAppService;
            _rulesService = rulesService;
        }
        public async Task<IActionResult> Index()
        {
            GetWarehouseInput getWarehouseInput = new GetWarehouseInput();
            var rulesCategory = await _rulesService.GetAllCategory();
            var rulesGroup = await _rulesService.GetAllGroup();
            var rulesKind = await _rulesService.GetAllKind();

            var dto = await _wareHouseAppService.GetAll(getWarehouseInput);
            List<SelectListItem> listItems = new List<SelectListItem>();

            WarehouseViewModel model = new WarehouseViewModel
            {
                WarehouseList = dto.Items.ToList().OrderBy(x => x.Name).ToList(),
                rulesCategory = rulesCategory,
                rulesGroup = rulesGroup,
                rulesKind = rulesKind
            };
            return View(model);
        }
    }
}
