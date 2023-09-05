using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using bbk.netcore.Authorization;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Ruleses;
using bbk.netcore.mdl.OMS.Application.Units;
using bbk.netcore.mdl.OMS.Application.WarehouseItems;
using bbk.netcore.mdl.OMS.Application.WarehouseItems.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses;
using bbk.netcore.mdl.OMS.Application.WarehouseTypes;
using bbk.netcore.Web.Areas.Inventorys.Models.WareHouse;
using bbk.netcore.Web.Areas.Inventorys.Models.WareHouseItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class WareHouseController : netcoreControllerBase
    {
        private readonly IWareHouseAppService _wareHouseAppService;
        private readonly IWarehouseTypesService _warehouseTypesService;
        private readonly IWarehouseItemAppService _warehouseItemAppService;
        private readonly IUnitService _unitService;
        private readonly IRulesService _rulesService;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        public WareHouseController(IWareHouseAppService wareHouseAppService, IWarehouseTypesService warehouseTypesService, IWarehouseItemAppService warehouseItemAppService, IUnitService unitService, 
            IRulesService rulesService,
            UserManager userManager,
            RoleManager roleManager)
        {
            _wareHouseAppService = wareHouseAppService;
            _warehouseTypesService = warehouseTypesService;
            _warehouseItemAppService= warehouseItemAppService;
            _unitService = unitService;
            _rulesService = rulesService;
            _userManager = userManager;
            _roleManager= roleManager;
        }

        
        public async Task<IActionResult> Index()
        {

            var User = _userManager.Users.Where(x => x.Id == AbpSession.UserId).FirstOrDefault();
            var roleNames = await _userManager.GetRolesAsync(User);
            var roleName = _roleManager.Roles.Where(x => x.Name == roleNames.FirstOrDefault()).Select(x => x.DisplayName).FirstOrDefault();
            WareHouseAddressModel model = new WareHouseAddressModel
            {
                rolename = roleName
            };
            return View(model);
        }

    
        public async Task<PartialViewResult> CreateWareHouse()
        {
            var province = await _wareHouseAppService.GetAddress("province.json", null);
            var listwarehouseTypes = await _warehouseTypesService.GetAllWarehouseType();

            WareHouseAddressModel model = new WareHouseAddressModel
            {
                provinces = province,
                ListWarehouseType = listwarehouseTypes
            };
            return PartialView("_CreateModal",model);
        }
       
        public async Task<IActionResult> EditWareHouseModal(long Id)
        {
            var dto = await _wareHouseAppService.GetAsync(new EntityDto<long>(Id));
            var listwarehouseTypes = await _warehouseTypesService.GetAllWarehouseType();
            var province = await _wareHouseAppService.GetAddress("province.json", null);
            WareHouseAddressModel model = new WareHouseAddressModel
            {
                provinces = province,
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                CityId = dto.CityId,
                DistrictId = dto.DistrictId,
                WardsId = dto.WardsId,
                Number = dto.Number,
                Lattitude = dto.Lattitude,
                Longitude = dto.Longitude,
                Description = dto.Description,
                TypeCode = dto.TypeCode,
                DeleteFlag = dto.DeleteFlag,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,
                ListWarehouseType = listwarehouseTypes
            };
            return PartialView("_EditModal", model);
        }

        public async Task<IActionResult> ViewWareHouse(long warehouseId)
        {
            var dto = await _wareHouseAppService.GetAsync(new EntityDto<long>(warehouseId));
            var listwarehouseTypes = await _warehouseTypesService.GetAllWarehouseType();
            var province = await _wareHouseAppService.GetAddress("province.json", null);
            var User = _userManager.Users.Where(x => x.Id == AbpSession.UserId).FirstOrDefault();
            var roleNames = await _userManager.GetRolesAsync(User);
            var roleName = _roleManager.Roles.Where(x => x.Name == roleNames.FirstOrDefault()).Select(x => x.DisplayName).FirstOrDefault();
            WareHouseAddressModel model = new WareHouseAddressModel
            {
                provinces = province,
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                CityId = dto.CityId,
                DistrictId = dto.DistrictId,
                WardsId = dto.WardsId,
                Number = dto.Number,
                Lattitude = dto.Lattitude,
                Longitude = dto.Longitude,
                Description = dto.Description,
                TypeCode = dto.TypeCode,
                DeleteFlag = dto.DeleteFlag,
                ListWarehouseType = listwarehouseTypes,
                rolename = roleName,
            };
            return View("ViewWareHouse", model);
        }

        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }


        /// code Kien
        public async Task<PartialViewResult> CreateWarhouseItem()
        {
            var unitList = await _unitService.GetUnitListDtos();
            var rulesCategory = await _rulesService.GetAllCategory();
            WarehouseItemUnitModel modal = new WarehouseItemUnitModel
            {
                unitListDtos = unitList,
                rulesCategory = rulesCategory,
            };
            return PartialView("CreateItem",modal);
        }
        public async Task<IActionResult> CreateWarhouseItemSub(int Id)
        {
            var Dto = await _warehouseItemAppService.GetById(new EntityDto(Id));
            var unitList = await _unitService.GetUnitListDtos();
            var rulesCategory = await _rulesService.GetAllCategory();
            WarehouseItemUnitModel modal = new WarehouseItemUnitModel
            {
                unitListDtos = unitList,
                warehouseItem = Dto,
                rulesCategory = rulesCategory,
            };
            return PartialView("CreateItemSub", modal);
        }
        public async Task<IActionResult> EditWarhouseItem(int Id)
        {
            var Dto = await _warehouseItemAppService.GetById(new EntityDto(Id));
            var unitList = await _unitService.GetUnitListDtos();
            var rulesCategory = await _rulesService.GetAllCategory();
            WarehouseItemUnitModel modal = new WarehouseItemUnitModel
            {
                unitListDtos = unitList,
                warehouseItem = Dto,
                rulesCategory = rulesCategory,
            };
            return PartialView("EditItem", modal);
        }

        public async Task<IActionResult> ViewWareHouseLayout(long Id)
        {
            var dto = await _wareHouseAppService.GetAsync(new EntityDto<long>(Id));
            var listwarehouseTypes = await _warehouseTypesService.GetAllWarehouseType();
            var User = _userManager.Users.Where(x => x.Id == AbpSession.UserId).FirstOrDefault();
            var roleNames = await _userManager.GetRolesAsync(User);
            var roleName = _roleManager.Roles.Where(x => x.Name == roleNames.FirstOrDefault()).Select(x => x.DisplayName).FirstOrDefault();
            WareHouseAddressModel model = new WareHouseAddressModel
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                CityId = dto.CityId,
                DistrictId = dto.DistrictId,
                WardsId = dto.WardsId,
                Number = dto.Number,
                Lattitude = dto.Lattitude,
                Longitude = dto.Longitude,
                Description = dto.Description,
                TypeCode = dto.TypeCode,
                DeleteFlag = dto.DeleteFlag,
                ListWarehouseType = listwarehouseTypes,
                Width = dto.Width,
                Height = dto.Height,
                Length = dto.Length,
                rolename = roleName
            };
            return View(model);
        }
    }
}
