using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.ExportRequestDetails;
using bbk.netcore.mdl.OMS.Application.ExportRequests;
using bbk.netcore.mdl.OMS.Application.Itemses;
using bbk.netcore.mdl.OMS.Application.Subsidiaries;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Application.Transfers;
using bbk.netcore.mdl.OMS.Application.WareHouses;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.Web.Areas.Inventorys.Models.ExportRequests;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ExportRequestsController : netcoreControllerBase
    {
        private readonly IWareHouseAppService _wareHouseAppService;
        private readonly ISupplierAppService _supplierAppService;
        private readonly IItemsService _itemAppService;
        private readonly UserManager _userManager;
        private readonly IExportRequests _exportRequests;
        private readonly IExportRequestDetails _exportRequestDetails;
        private readonly ITransferAppService _transferAppService;
        private readonly ISubsidiaryService _subsidiaryService;
        private readonly RoleManager _role;
        private readonly IRepository<UserRole, long> _userrole;

        public ExportRequestsController(IWareHouseAppService wareHouseAppService,
             ISupplierAppService supplierAppService,
             IItemsService itemsService,
             UserManager userManager,
             IExportRequests exportRequests,
             IExportRequestDetails exportRequestDetails,
             ITransferAppService transferAppService,
             ISubsidiaryService subsidiaryService,
                RoleManager role,
                IRepository<UserRole, long> userrole
             )
        {
            _wareHouseAppService = wareHouseAppService;
            _supplierAppService = supplierAppService;
            this._userManager = userManager;
            _itemAppService = itemsService;
            _exportRequests = exportRequests;
            _exportRequestDetails = exportRequestDetails;   
            _transferAppService = transferAppService;
            _subsidiaryService = subsidiaryService; 
            _role = role;
            _userrole = userrole;
        }

        public async Task<ActionResult> Index()
        {
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                WarehouseList = warehouseList.ToList()
            };
            return View(ExportRequestModel);
        }
        public async Task<PartialViewResult> CreateExportRequests()
        {
            var supplierList = await _supplierAppService.GetSupplierList();
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var itemList = await _itemAppService.GetItemList();
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            ViewBag.username = User.FullName;
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                ItemsListDtos = itemList,
                SuppliersList = supplierList,
                WarehouseList = warehouseList.ToList()

            };
            return PartialView("_CreateModal", ExportRequestModel);
        }
        public async Task<PartialViewResult> EditExportRequests(long id)
        {
            var supplierList = await _supplierAppService.GetSupplierList();
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var itemList = await _itemAppService.GetItemList();
            var exportRequest = await _exportRequests.GetAsync(new EntityDto<long>(id));
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            var UserCreate = _userManager.Users.Where(x => x.Id == exportRequest.CreatorUserId).FirstOrDefault();
            ViewBag.username = User.FullName;
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                exportRequests = exportRequest,
                ItemsListDtos = itemList,
                SuppliersList = supplierList,
                WarehouseList = warehouseList.ToList(),
                CreatedByName = UserCreate.FullName

            };
            return PartialView("_EditModal", ExportRequestModel);
        }
        public async Task<IActionResult> ViewDetails(long Id)
        {
            var dto = await _exportRequests.GetAsync(new EntityDto<long>(Id));
            var supplierList =  _supplierAppService.GetSupplierList().Result.Where(x=>x.Id == dto.SupplierId).ToList();
            var warehouseList = _wareHouseAppService.GetWarehouseList().Result.Where(x => x.Id == dto.WarehouseDestinationId).FirstOrDefault();

            //string rolename = "Trưởng phòng mua hàng";
            //var roles = _role.Roles.FirstOrDefault(x => x.DisplayName == rolename);
            //var userrole = _userrole.FirstOrDefault(x => x.RoleId == roles.Id);
            //var user = _userManager.Users.FirstOrDefault(x => x.Id == userrole.UserId);
            //string email = user.EmailAddress;
            //string name = user.Name;
            //ViewBag.email = email;
            //ViewBag.name = name;


            var UserCreate = _userManager.Users.Where(x => x.Id == dto.CreatorUserId).FirstOrDefault();
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                exportRequests = dto,
                SuppliersList = supplierList,
                warehouse = warehouseList,
                CreatedByName = UserCreate.FullName

            };
            return View("ViewDetails", ExportRequestModel);
        }
       

        public async Task<IActionResult> Print(long Id)
        {
            var dto = await _exportRequests.GetAsync(new EntityDto<long>(Id));
            var supplierList = _supplierAppService.GetSupplierList().Result.Where(x => x.Id == dto.SupplierId).ToList();
            var warehouseList = _wareHouseAppService.GetWarehouseList().Result.Where(x => x.Id == dto.WarehouseDestinationId).FirstOrDefault();
            var UserCreate = _userManager.Users.Where(x => x.Id == dto.CreatorUserId).FirstOrDefault();
            // var UserUpdate = _userManager.Users.FirstOrDefault(x => x.Id == dto.LastModifierUserId);
            var Name = _supplierAppService.GetSupplierList().Result.Where(x => x.Id == dto.SupplierId).FirstOrDefault();
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                exportRequests = dto,
                SuppliersList = supplierList,
                warehouse = warehouseList,
                CreatedByName = UserCreate.FullName

            };
            return View("Print", ExportRequestModel);
        }


        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns>View phiếu xuất kho của thủ kho</returns>
        public async Task<IActionResult> Export()
        {
            return View("Export");
        }
        public async Task<IActionResult> UpdateExport()
        {
            return View("UpdateExport");
        }

        public async Task<PartialViewResult> CreateExport(long id)
        {
            GetWarehouseInput getWarehouseInput = new GetWarehouseInput();
            var dto = await _wareHouseAppService.GetAll(getWarehouseInput);
            var supplierList = await _supplierAppService.GetSupplierList();
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var itemList = await _itemAppService.GetItemList();
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            ViewBag.username = User.FullName;
            var tran = await _transferAppService.GetAsync(new EntityDto<long>(id));
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                ItemsListDtos = itemList,
                SuppliersList = supplierList,
                WarehouseList = dto.Items.ToList(),
                transferListDto = tran,
                
            };
            return PartialView("_CreateExport", ExportRequestModel);
        }
          public async Task<PartialViewResult> UpdateExportModal(long id)
        {
            var supplierList = await _supplierAppService.GetSupplierList();
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var itemList = await _itemAppService.GetItemList();
            var exportRequest = await _exportRequests.GetAsync(new EntityDto<long>(id));
            var UserCreate = _userManager.Users.Where(x => x.Id == exportRequest.CreatorUserId).FirstOrDefault();
            //var tran = await _transferAppService.GetAsync(new EntityDto<long>(id));
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                ItemsListDtos = itemList,
                SuppliersList = supplierList,
                WarehouseList = warehouseList.ToList(),
                exportRequests = exportRequest,
                CreatedByName = UserCreate.FullName
                //  transferListDto = tran,

            };
            return PartialView("_UpdateExport", ExportRequestModel);
        }

        public async Task<PartialViewResult> CreateExportrequirement()
        {
            var supplierList = await _supplierAppService.GetSupplierList();
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var itemList = await _itemAppService.GetItemList();
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            var subsidiary = await _subsidiaryService.GetSubsidiaryList();
            ViewBag.username = User.FullName;
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                ItemsListDtos = itemList,
                SuppliersList = supplierList,
                WarehouseList = warehouseList.ToList(),
                SubsidiaryList = subsidiary ,

            };
            return PartialView("_CreateExportRequirement", ExportRequestModel);
        }

        public async Task<PartialViewResult> EditExportrequirement(long id)
        {
            var supplierList = await _supplierAppService.GetSupplierList();
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var itemList = await _itemAppService.GetItemList();
            var exportRequest = await _exportRequests.GetAsync(new EntityDto<long>(id));
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            var UserCreate = _userManager.Users.Where(x => x.Id == exportRequest.CreatorUserId).FirstOrDefault();
            var subsidiary = await _subsidiaryService.GetSubsidiaryList();
            ViewBag.username = User.FullName;
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                exportRequests = exportRequest,
                ItemsListDtos = itemList,
                SuppliersList = supplierList,
                WarehouseList = warehouseList.ToList(),
                CreatedByName = UserCreate.FullName,
                SubsidiaryList = subsidiary,

            };
            return PartialView("_EditExportRequirement", ExportRequestModel);
        }

        public async Task<PartialViewResult> CreateExportCraft(long id)
        {
            var supplierList = await _supplierAppService.GetSupplierList();
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var itemList = await _itemAppService.GetItemList();
            var exportRequest = await _exportRequests.GetAsync(new EntityDto<long>(id));
         //   var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            var UserCreate = _userManager.Users.Where(x => x.Id == exportRequest.CreatorUserId).FirstOrDefault();
            var subsidiary = await _subsidiaryService.GetSubsidiaryList();
            ViewBag.username = UserCreate.FullName;
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                exportRequests = exportRequest,
                ItemsListDtos = itemList,
                SuppliersList = supplierList,
                WarehouseList = warehouseList.ToList(),
                CreatedByName = UserCreate.FullName,
                SubsidiaryList = subsidiary,

            };
            return PartialView("_CreateExportCraft", ExportRequestModel);
        }

        public async Task<ActionResult> ExportRequirement()
        {
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                WarehouseList = warehouseList.ToList()
            };
            return View(ExportRequestModel);
        }

        public async Task<IActionResult> ExportRequirementDetail(long Id)
        {
            var dto = await _exportRequests.GetAsync(new EntityDto<long>(Id));
            var supplierList = _supplierAppService.GetSupplierList().Result.Where(x => x.Id == dto.SupplierId).ToList();
            var warehouseList = _wareHouseAppService.GetWarehouseList().Result.Where(x => x.Id == dto.WarehouseDestinationId).FirstOrDefault();

            string rolename = "Trưởng phòng mua hàng";
            var roles = _role.Roles.FirstOrDefault(x => x.DisplayName == rolename);
            var userrole = _userrole.FirstOrDefault(x => x.RoleId == roles.Id);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userrole.UserId);
            string email = user.EmailAddress;
            string name = user.Name;
            ViewBag.email = email;
            ViewBag.name = name;

            var UserCreate = _userManager.Users.Where(x => x.Id == dto.CreatorUserId).FirstOrDefault();
            var subsidiary = await _subsidiaryService.GetAsync(new EntityDto((int)dto.SubsidiaryId));
            ExportRequestModel ExportRequestModel = new ExportRequestModel
            {
                exportRequests = dto,
                SuppliersList = supplierList,
                warehouse = warehouseList,
                CreatedByName = UserCreate.FullName,
                subsidiaryDto = subsidiary 

            };
            return View("ExportRequirementDetail", ExportRequestModel);
        }

        public async Task<IActionResult> EditModal(long Id)
        {
            ViewBag.Id = Id;

            return PartialView("_CreateModalReject");
        }

    }

}
