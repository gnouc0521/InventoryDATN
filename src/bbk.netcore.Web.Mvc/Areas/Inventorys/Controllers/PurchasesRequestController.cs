using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises;
using bbk.netcore.mdl.OMS.Application.Subsidiaries;
using bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto;
using bbk.netcore.Web.Areas.Inventorys.Models.PurchsesRequest;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class PurchasesRequestController : netcoreControllerBase
    {
        private readonly IPurchasesRequestsService _purchasesRequestsService;
        private readonly ISubsidiaryService _subsidiaryService;
        private readonly IPurchasesSynthesiseAppService _purchasesSynthesiseAppService;
        private readonly UserManager _userManager;
        private readonly RoleManager _role;
        private readonly IRepository<UserRole, long> _userrole;
        public PurchasesRequestController(IPurchasesRequestsService purchasesRequestsService,
                                          ISubsidiaryService subsidiaryService,
                                          IPurchasesSynthesiseAppService purchasesSynthesiseAppService,
                                        UserManager userManager,
                                        RoleManager role,
                                       IRepository<UserRole, long> userrole)
        {
            _purchasesRequestsService = purchasesRequestsService;
            _subsidiaryService= subsidiaryService;
            _purchasesSynthesiseAppService = purchasesSynthesiseAppService;
            _userManager = userManager;
            _role= role;
            _userrole = userrole;
        }

        public IActionResult Index()
        {
            return View();
        }


        public  async Task<PartialViewResult> CreatePurchasesRequest()
        {
            var subsidiaryList = await _subsidiaryService.GetSubsidiaryList();
            PurchsesRequestViewModel model = new PurchsesRequestViewModel
            {
                subsidiaryListDto = subsidiaryList
            };
            return PartialView("_CreateModal",model);
        }

        public async Task<PartialViewResult> UpdatePurchasesRequest(int Id)
        {
            var dto = await _purchasesRequestsService.GetAsync(new EntityDto(Id));
            var subsidiaryList = await _subsidiaryService.GetSubsidiaryList();
            PurchsesRequestViewModel model = new PurchsesRequestViewModel
            {
                subsidiaryListDto = subsidiaryList,
                purchasesRequest = dto,
            };
            return PartialView("_EditModal",model);
        }

        public async Task<IActionResult> ViewDetails(int Id)
        {
            var dto = await _purchasesRequestsService.GetAsync(new EntityDto(Id));
            var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            SubsidiarySearch sub = new SubsidiarySearch();
            var sunInfo = _subsidiaryService.GetSubsidiaryList().Result.Where(x=>x.Id == dto.SubsidiaryCompanyId).FirstOrDefault();
            PurchsesRequestViewModel model = new PurchsesRequestViewModel
            {
                purchasesRequest= dto,
                subsidiaryList = sunInfo,
                RequestStatus = dto.RequestStatus,
                CityId = sunInfo.CityId,
                DistrictId= sunInfo.DistrictId,
                WardsId= sunInfo.WardsId,
            };
            return View("ViewDetails", model);
        }


        public async Task<IActionResult> EditModal(long PurchasesSynthesiseId)
        {
            ViewBag.Id = PurchasesSynthesiseId;

            return PartialView("_CreateModalReject");
        }

        public IActionResult Downloadfile()
        {
            var memory = DownloadSinghFile("PhieuYC.xlsx", "wwwroot\\data\\tenants\\1\\Templates");
            return File(memory.ToArray(), "application/vnd.ms-excel", "PhieuYC.xlsx");
        }
        private MemoryStream DownloadSinghFile(string filename, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;

            }
            memory.Position = 0;
            return memory;
        }


        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }

        public async Task<ActionResult> DetailSynthesise(long SynthesiseId)
        { 
           var model = await _purchasesSynthesiseAppService.GetAsync(new EntityDto<long>(SynthesiseId));
            string rolename = "Trưởng phòng kế hoạch";
            var roles = _role.Roles.FirstOrDefault(x=>x.DisplayName == rolename);
            var userrole = _userrole.FirstOrDefault(x => x.RoleId == roles.Id);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userrole.UserId);
            string email = user.EmailAddress;
            string name = user.Name;
            ViewBag.email = email;
            ViewBag.name = name;
            ViewBag.SynthesiseId = SynthesiseId;    
            return View(model);
           // return View();
        }


    }

}
