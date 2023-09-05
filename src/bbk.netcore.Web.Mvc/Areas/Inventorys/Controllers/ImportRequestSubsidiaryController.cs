using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Contracts;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys;
using bbk.netcore.mdl.OMS.Application.Orders;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Application.WareHouses;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.Web.Areas.Inventorys.Models.IMBSub;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ImportRequestSubsidiaryController : netcoreControllerBase
    {
        private readonly IImportRequestSubidiaryService _importRequestSubidiaryService;
        private readonly UserManager _userManager;
        private readonly IWareHouseAppService _wareHouseAppService;
        private readonly ISupplierAppService _supplierAppService;
        private readonly IOrderAppService _orderAppService;
        private readonly IContractAppService _contractAppService;
        private readonly RoleManager _role;
        private readonly IRepository<UserRole, long> _userrole;
        public ImportRequestSubsidiaryController(IImportRequestSubidiaryService importRequestSubidiaryService,
                UserManager userManager,
                IWareHouseAppService wareHouseAppService,
                ISupplierAppService supplierAppService,
                IOrderAppService orderAppService,
                IContractAppService contractAppService,
                RoleManager role,
                IRepository<UserRole, long> userrole)
        {
            _importRequestSubidiaryService = importRequestSubidiaryService;
            _userManager = userManager;
            _wareHouseAppService = wareHouseAppService;
            _supplierAppService = supplierAppService;
            _orderAppService = orderAppService;
            _contractAppService = contractAppService;
            _role = role;
            _userrole = userrole;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateImportRequest(long Id)
        {
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var supplierList = await _supplierAppService.GetSupplierList();
            var order = await _orderAppService.GetAsync(new EntityDto<long>(Id));
            var contract = await _contractAppService.GetAsync(new EntityDto(((int)order.ContractId)));

            IndexViewModel model = new IndexViewModel
            {
                OrderId = Id,
                WarehouseList = warehouseList,
                CreatedBy = User.FullName,
                Suppliers = supplierList,
                IdSupplier = contract.SupplierId,
                OrderCode = order.OrderCode,

            };
            return PartialView("_CreateModal", model);
        }



        public async Task<IActionResult> ViewDetails(int Id)
        {
            var dto = await _importRequestSubidiaryService.GetAsync(new EntityDto(Id));
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
            var supplierList = _supplierAppService.GetSupplierList().Result.Where(x => x.Id == dto.SuppilerId).FirstOrDefault();
            IndexViewModel model = new IndexViewModel
            {
                subListDto = dto,
                NameWareHouse = warehouseList.Name,
                CreatedBy = UserCreate.FullName,
                NameNCC = supplierList.Name,
                IdSupplier = dto.SuppilerId
            };
            return View("ViewDetails", model);
        }

        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }

        public async Task<IActionResult> EditModal(long Id)
        {
            ViewBag.Id = Id;

            return PartialView("_CreateModalReject");
        }
    }
}
