using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Transfers;
using bbk.netcore.mdl.OMS.Application.WareHouses;
using bbk.netcore.Web.Areas.Inventorys.Models.Transfer;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class TransferController : netcoreControllerBase
    {
        private readonly IWareHouseAppService _wareHouseAppService;
        private readonly UserManager _userManager;
        private readonly ITransferAppService _transferAppService;
        private readonly RoleManager _role;
        private readonly IRepository<UserRole, long> _userrole;
        public TransferController(IWareHouseAppService wareHouseAppService, UserManager userManager, ITransferAppService transferAppService, RoleManager role,
                IRepository<UserRole, long> userrole)
        {
            _wareHouseAppService = wareHouseAppService;
            _userManager = userManager;
            _transferAppService = transferAppService;
            _role = role;
            _userrole = userrole;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TransferManagement()
        {
            return View();
        }

        public async Task<IActionResult> EditModal(long Id)
        {
            ViewBag.Id = Id;

            return PartialView("_CreateModalReject");
        }

        public async Task<IActionResult> ViewDetails(long Id)
        {
            var dto = await _transferAppService.GetAsync(new EntityDto<long>(Id));

            string rolename = "Trưởng phòng kế hoạch";
            var roles = _role.Roles.FirstOrDefault(x => x.DisplayName == rolename);
            var userrole = _userrole.FirstOrDefault(x => x.RoleId == roles.Id);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userrole.UserId);
            string email = user.EmailAddress;
            string name = user.Name;
            ViewBag.email = email;
            ViewBag.name = name;


            var UserCreate = _userManager.Users.Where(x => x.Id == dto.CreatorUserId).FirstOrDefault();

            TransferViewModel model = new TransferViewModel
            {
                TransferList = dto,
                CreatedBy = UserCreate.FullName,

            };

            return View("ViewDetails", model);
        }

        public async Task<PartialViewResult> CreateTransfer()
        {
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            TransferViewModel model = new TransferViewModel
            {
                WarehouseList = warehouseList,
                CreatedBy = User.FullName,

            };
            return PartialView("_CreateModal", model);
        }

        public async Task<PartialViewResult> EditTransfer(long Id)
        {
            var dto = await _transferAppService.GetAsync(new EntityDto<long>(Id));
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            var UserCreate = _userManager.Users.Where(x => x.Id == dto.CreatorUserId).FirstOrDefault();

            TransferViewModel model = new TransferViewModel
            {
                TransferList = dto,
                CreatedBy = UserCreate.FullName,
                WarehouseList = warehouseList,
            };
           
            return PartialView("_EditTransfer", model);
        }

        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }
    }
}
