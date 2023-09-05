using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Timing;
using Abp.Web.Models;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Contracts;
using bbk.netcore.mdl.OMS.Application.Orders;
using bbk.netcore.mdl.OMS.Application.PurchaseAssignments;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.SendMailSuppliers.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.Web.Areas.Inventorys.Models.Items;
using bbk.netcore.Web.Areas.Inventorys.Models.MyWork;
using bbk.netcore.Web.Areas.Inventorys.Models.Order;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class MyWorkController : netcoreControllerBase
    {
        private readonly IPurchasesSynthesiseAppService _purchasesSynthesiseAppService;
        private readonly IPurchaseAssignmentService _purchasesAssigmentAppService;
        private readonly ISupplierAppService _isupplierAppService;
        private readonly IWebHostEnvironment _iWebHostEnvironment;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        private readonly ISendMailService _sendMailService;
        private readonly ISupplierAppService _supplierAppService;
        private readonly UserManager _userManager;
        private readonly IContractAppService _ContractAppServiceService;
        private readonly IOrderAppService _OrderAppService;
        private readonly IUserAppService _userAppService;
        public MyWorkController( IPurchasesSynthesiseAppService purchasesSynthesiseAppService , 
                                 IPurchaseAssignmentService purchasesAssigmentAppService, 
                                 ISupplierAppService isupplierAppService,
                                 IWebHostEnvironment iWebHostEnvironment,
                                 IFileSystemBlobProvider fileSystemBlobProvider,
                                 ISendMailService sendMailService, ISupplierAppService supplierAppService, UserManager userManager,
                                 IContractAppService ContractAppServiceService,
                                 IOrderAppService OrderAppService,
                                 IUserAppService userAppService)
        {
            _purchasesSynthesiseAppService = purchasesSynthesiseAppService;
            _purchasesAssigmentAppService= purchasesAssigmentAppService;
            _isupplierAppService= isupplierAppService;
            _fileSystemBlobProvider = fileSystemBlobProvider;
            _iWebHostEnvironment= iWebHostEnvironment;
            _sendMailService= sendMailService;
            _supplierAppService= supplierAppService;
            _userManager= userManager;
            _ContractAppServiceService = ContractAppServiceService;
            _OrderAppService = OrderAppService;
            _userAppService = userAppService;
        }
        public  ActionResult Index()
        {
            return View();
        }  
        
        
        public  ActionResult Purchase()
        {
            return View();
        }

        public async Task<PartialViewResult> Sendmail()
        {
            var supplierList = await _isupplierAppService.GetSupplierList();

            SendMailListDto model = new SendMailListDto
            {
                Suppliers = supplierList
            };

            return PartialView("Sendmail",model);
        }

        public async Task<ActionResult> Update(int Id)
        {
            var dto = await _purchasesSynthesiseAppService.GetAsync(new EntityDto<long>(Id));
            PurchasesSynthesisListDto model = new PurchasesSynthesisListDto
            {
                Id= dto.Id,
                PurchasesSynthesiseId = Id
            };
            return View("_ViewDetail", model);
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocument(SendMailListDto model)
        {
            try
            {
                string webRootPath = _iWebHostEnvironment.WebRootPath;
                string contentRootPath = _iWebHostEnvironment.ContentRootPath;
                var sup = _supplierAppService.GetAllSupplier().Result.Items.FirstOrDefault(x => x.Id == model.SupplierId);
                var userName = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);   
                model.FileUpload = Request.Form.Files.ToList();
                string path = "";
                FileInfo fileInfo = new FileInfo(model.FileUpload[0].FileName);
                var filePath = Path.Combine(Path.GetFileName(fileInfo.Name.ToString().Replace(fileInfo.Extension, "")) + "_"+ sup.Name + "_" + Clock.Now.ToString("yyyy_MM_dd_mm") +fileInfo.Extension);

                string fullFilePath;
                using (var stream = model.FileUpload[0].OpenReadStream())
                {
                    fullFilePath = await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(filePath, stream));

                }
                path = webRootPath + Path.DirectorySeparatorChar.ToString() + "templateEmail" + Path.DirectorySeparatorChar.ToString() + "sendMail.html";
                var pathImg = webRootPath + Path.DirectorySeparatorChar.ToString() + "templateEmail" + Path.DirectorySeparatorChar.ToString() + "Logo_Text.png";
                path = System.IO.File.ReadAllText(path);

                var file = Request.Form.Files.ToList();
                model.NameSup = sup.Name;
                model.EmailSup = sup.Email;
                model.SubJect = "Công ty BBK SOLUTION – Đơn Đặt Hàng";
                path = path.Replace("{{ImgSrc}}", pathImg);
                path = path.Replace("{{TenNCC}}", model.NameSup);
                path = path.Replace("{{Nội dung email}}", model.Comment);
                await _sendMailService.SendEmailAsync(model.EmailSup, model.SubJect, path.ToString(),fullFilePath);
                //  }
                return Json(new AjaxResponse(new { data = model.NameSup }));

                //  return View("Index.cshtml");

            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        public async Task<ActionResult> OrderDetail(long id)
        {
            var model = await _OrderAppService.GetAsync(new EntityDto<long>(id));
            var contract = await _ContractAppServiceService.GetAsync(new EntityDto((int)model.ContractId));
            var supplier = await _supplierAppService.GetAsync(new EntityDto((int)contract.SupplierId));

            GetUsersInput getUsersInput = new GetUsersInput();
            getUsersInput.MaxResultCount = 500;
            var user = _userAppService.GetUsers(getUsersInput).Result.Items.Where(x => x.Id == model.UserId).ToList();
            ViewBag.UserName = user[0].Surname + " " + user[0].Name;
            OrderViewModel contractModel = new OrderViewModel
            {
                Contract = contract,
                orderListDto = model,
                Supplier = supplier,
            };
            return View("PurchaseDetail",contractModel);
        }

        public async Task<PartialViewResult> CreateExport(long id)
        {
            ViewBag.Id = id;
            var listsupplier = await _isupplierAppService.GetSupplierList();
            MyWorkViewModel myWorkView = new MyWorkViewModel
            {
                Suppliers = listsupplier
            };
            return PartialView("_CreateExport", myWorkView);
        }

        public IActionResult Downloadfile()
        {
            var memory = DownloadSinghFile("Baogia.xlsx", "wwwroot\\data\\tenants\\1\\Templates");
            return File(memory.ToArray(), "application/vnd.ms-excel", "Baogia.xlsx");
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
    }
}
