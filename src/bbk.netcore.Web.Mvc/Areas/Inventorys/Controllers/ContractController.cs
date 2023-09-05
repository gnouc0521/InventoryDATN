using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Contracts;
using bbk.netcore.mdl.OMS.Application.Contracts.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises;
using bbk.netcore.Web.Areas.Inventorys.Models.ImportRequest;
using bbk.netcore.Web.Areas.Inventorys.Models.Items;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ContractController : netcoreControllerBase
    {
        private readonly IContractAppService _icontractAppService;
        private readonly IQuotesSynthesiseAppService _quotesSynthesiseAppService;
        private readonly UserManager _userManager;
        private readonly RoleManager _role;
        private readonly IRepository<UserRole, long> _userrole;

        public ContractController(ContractAppService icontractAppService,
            IQuotesSynthesiseAppService quotesSynthesiseAppService,
            UserManager userManager,
            RoleManager role,
            IRepository<UserRole, long> userrole
            )
        {
            _icontractAppService= icontractAppService;
            _quotesSynthesiseAppService= quotesSynthesiseAppService;
            _userManager= userManager;
            _role= role;
            _userrole= userrole;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ApproveContract()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateContract(int Id)
        {
            var dto = await _quotesSynthesiseAppService.GetQuoteApprove(Id);
            return PartialView("_CreateModal", dto);
        }
        public async Task<IActionResult> EditModal(long Id)
        {
           ViewBag.Id = Id;

            return PartialView("_CreateModalReject");
        }

        public async Task<IActionResult> Print(int Id)
        {
            var dto = await _icontractAppService.GetAsync(new EntityDto(Id));

            string rolename = "Trưởng phòng mua hàng";
            var roles = _role.Roles.FirstOrDefault(x => x.DisplayName == rolename );
            var userrole = _userrole.FirstOrDefault(x => x.RoleId == roles.Id);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userrole.UserId);
            string email = user.EmailAddress;
            string name = user.Name;
            ViewBag.email = email;
            ViewBag.name = name;



            string rolename2 = "Giám đốc";
            var roleGD = _role.Roles.FirstOrDefault(x => x.DisplayName == rolename2);
            var userroleGD = _userrole.FirstOrDefaultAsync(x=>x.RoleId == roleGD.Id);
            var userGD = _userManager.Users.FirstOrDefault(x => x.Id == userroleGD.Result.UserId);
            string emailGD = userGD.EmailAddress;
            string nameGD = userGD.Name;
            ViewBag.emailGD = emailGD;
            ViewBag.nameGD = nameGD;

            return View("Print", dto);
        }

        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }
    }
}
