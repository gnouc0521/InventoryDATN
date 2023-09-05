using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using bbk.netcore.Authorization;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.ImportRequests;
using bbk.netcore.mdl.OMS.Application.InventoryTickets;
using bbk.netcore.mdl.OMS.Application.Itemses;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Application.Units;
using bbk.netcore.mdl.OMS.Application.WareHouses;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.Web.Areas.Inventorys.Models.ImportRequest;
using bbk.netcore.Web.Areas.Inventorys.Models.InventoryTicket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class InventoryTicketController : netcoreControllerBase
    {
        private readonly IImportRequestAppService _importRequestAppService;
        private readonly ISupplierAppService _supplierAppService;
        private readonly IWareHouseAppService _wareHouseAppService;
        private readonly IItemsService _itemAppService;
        private readonly UserManager _userManager;
        private readonly IUnitService _unitService;
        private readonly IInventoryTicketService _inventoryTicketService;
        private readonly RoleManager _roleManager;
        public InventoryTicketController(IImportRequestAppService importRequestAppService,
                                     ISupplierAppService supplierAppService,
                                     IWareHouseAppService wareHouseAppService,
                                     IItemsService itemAppService,
                                     UserManager userManager,
                                     IUnitService unitService,
                                     IInventoryTicketService inventoryTicketService,
                                      RoleManager roleManager)
        {
            _importRequestAppService = importRequestAppService;
            _supplierAppService = supplierAppService;
            _wareHouseAppService = wareHouseAppService;
            _userManager = userManager;
            _itemAppService = itemAppService;
            _unitService = unitService;
            _inventoryTicketService = inventoryTicketService;
            _roleManager= roleManager;
        }

       
        public async Task<IActionResult> Index()
        {
            GetWarehouseInput getWarehouseInput = new GetWarehouseInput();
            var dto = await _wareHouseAppService.GetAll(getWarehouseInput);
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var role in _userManager.Users)
                listItems.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.FullName });
            ViewBag.Name = listItems;
            var User = _userManager.Users.Where(x => x.Id == AbpSession.UserId).FirstOrDefault();
            var roleNames = await _userManager.GetRolesAsync(User);
            var roleName = _roleManager.Roles.Where(x => x.Name == roleNames.FirstOrDefault()).Select(x => x.DisplayName).FirstOrDefault();
            InventoryTicketViewModel model = new InventoryTicketViewModel
            {
                WarehouseList = dto.Items.ToList(),
                RoleName = roleName
            };
            return View(model);
        }

        public async Task<PartialViewResult> CreateImportRequest()
        {
           
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            InventoryTicketViewModel model = new InventoryTicketViewModel
            {
                WarehouseList = warehouseList,
                CreatedBy = User.FullName,

            };
            return PartialView("_CreateModal", model);
        }

        public async Task<IActionResult> EditImportRequestModal(int Id)
        {
            var dto = await _inventoryTicketService.GetAsync(new EntityDto(Id));
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            InventoryTicketViewModel model = new InventoryTicketViewModel
            {
                WarehouseList = warehouseList,
                CreatedBy = User.FullName,
                Code = dto.Code,
                StartDate = dto.StartDate,
                EndDate= dto.EndDate,
                CompleteTime= dto.CompleteTime,
                WarehouseId= dto.WarehouseId,
                Id = dto.Id
            };

            return PartialView("_EditModal", model);
        }

        public async Task<IActionResult> ViewDetails(int Id)
        {
            var dto = await _inventoryTicketService.GetAsync(new EntityDto(Id));
            var itemList = await _itemAppService.GetItemList();
            var warehouseList = _wareHouseAppService.GetWarehouseList().Result.Where(x => x.Id == dto.WarehouseId).FirstOrDefault() ;
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            InventoryTicketViewModel model = new InventoryTicketViewModel
            {
                NameWareHouse = warehouseList.Name,
                CreatedBy = User.FullName,
                Code = dto.Code,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CompleteTime = dto.CompleteTime,
                WarehouseId = dto.WarehouseId,
                Id = dto.Id
            };
            return View("ViewDetails", model);
        }

        public async Task<IActionResult> Print(int Id)
        {
            var dto = await _inventoryTicketService.GetAsync(new EntityDto(Id));
            var itemList = await _itemAppService.GetItemList();
            var warehouseList = _wareHouseAppService.GetWarehouseList().Result.Where(x => x.Id == dto.WarehouseId).FirstOrDefault();
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            InventoryTicketViewModel model = new InventoryTicketViewModel
            {
                NameWareHouse = warehouseList.Name,
                CreatedBy = User.FullName,
                Code = dto.Code,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CompleteTime = dto.CompleteTime,
                WarehouseId = dto.WarehouseId,
                Id = dto.Id
            };
            return View("Print", model);
        }

        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }


    }
}
