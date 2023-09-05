using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Models;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Itemses;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises;
using bbk.netcore.mdl.OMS.Application.Quotes;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.Subsidiaries;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.Web.Areas.Inventorys.Models.Quote;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class QuoteController : netcoreControllerBase
    {
        private readonly IPurchasesRequestsService _purchasesRequestsService;
        private readonly ISubsidiaryService _subsidiaryService;
        private readonly IPurchasesSynthesiseAppService _purchasesSynthesiseAppService;
        private readonly UserManager _userManager;
        private readonly ISendMailService _sendMailService;
        private readonly IWebHostEnvironment _iWebHostEnvironment;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        private readonly IQuotesService _quotesService;
        private readonly IItemsService _itemsService;
        private readonly ISupplierAppService _supplierAppService;
        private readonly IQuotesSynthesiseAppService _quotesSynthesiseAppService;
        private readonly RoleManager _role;
        private readonly IRepository<UserRole, long> _userrole;
        public QuoteController(IPurchasesRequestsService purchasesRequestsService,
                                          ISubsidiaryService subsidiaryService,
                                          IPurchasesSynthesiseAppService purchasesSynthesiseAppService,
                                        UserManager userManager,
                                        ISendMailService sendMailService,
                                        IWebHostEnvironment iWebHostEnvironment,
                                       IFileSystemBlobProvider fileSystemBlobProvider,
                                       IQuotesService quotesService,
                                       IItemsService itemsService,
                                       ISupplierAppService supplierAppService,
                                       IQuotesSynthesiseAppService quotesSynthesiseAppService,
                                       RoleManager role,
                                       IRepository<UserRole, long> userrole)
        {
            _purchasesRequestsService = purchasesRequestsService;
            _subsidiaryService = subsidiaryService;
            _purchasesSynthesiseAppService = purchasesSynthesiseAppService;
            _userManager = userManager;
            _sendMailService = sendMailService;
            _iWebHostEnvironment = iWebHostEnvironment;
            _fileSystemBlobProvider = fileSystemBlobProvider;
            _quotesService = quotesService;
            _itemsService = itemsService;
            _supplierAppService = supplierAppService;
            _quotesSynthesiseAppService = quotesSynthesiseAppService;
            _role = role;
            _userrole = userrole;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Compare()
        {
            return View();
        }
        public async Task<ActionResult> History()
        {
            return View();
        }

        public async Task<IActionResult> BrowseQuotes()
        {
            return View();
        }
        public async Task<ActionResult> HistoryQuoteDetail(long QuoteId)
        {
            var model = await _quotesService.GetAsync(new EntityDto<long>(QuoteId));

            ViewBag.SynthesiseId = QuoteId;
            return View(model);
        }
        public async Task<ActionResult> CompareDetail(long SynthesiseId)
        {
            var model = await _quotesSynthesiseAppService.GetAsync(new EntityDto<long>(SynthesiseId));
            string rolename = "Trưởng phòng mua hàng";
            var roles = _role.Roles.FirstOrDefault(x => x.DisplayName == rolename);
            var userrole = _userrole.FirstOrDefault(x => x.RoleId == roles.Id);
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userrole.UserId);
            string email = user.EmailAddress;
            string name = user.Name;
            ViewBag.email = email;
            ViewBag.name = name;
            ViewBag.SynthesiseId = SynthesiseId;
            return View(model);
        }



        public async Task<List<QuoteListDto>> ImportExcel(IFormFile file)
        {
            try
            {
                List<QuoteListDto> itemsModels = new List<QuoteListDto>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                        // get number of rows and columns in the sheet
                        int rows = workSheet.Dimension.Rows; // 20
                        int columns = workSheet.Dimension.Columns; // 7
                        // loop through the worksheet rows and columns
                        if (workSheet != null)
                        {
                            for (int i = 10; i < rows + 4; i++)
                            {
                                if (workSheet.Cells[i, 6].Value == null)
                                {
                                    break;
                                }
                                itemsModels.Add(new QuoteListDto
                                {
                                    ItemName = (workSheet.Cells[i, 2].Value ?? string.Empty).ToString(),
                                    SymbolCode = (workSheet.Cells[i, 3].Value ?? string.Empty).ToString(),
                                    Specifications = (workSheet.Cells[i, 4].Value ?? string.Empty).ToString(),
                                    UnitName = (workSheet.Cells[i, 5].Value ?? string.Empty).ToString(),
                                    QuotePrice = decimal.Parse((workSheet.Cells[i, 6].Value ?? string.Empty).ToString()),
                                    QuantityQuote = Int64.Parse((workSheet.Cells[i,7].Value??string.Empty).ToString()),
                                    Note = (workSheet.Cells[i, 8].Value ?? string.Empty).ToString(),
                                    SupplierName = (workSheet.Cells[7, 1].Value ?? string.Empty).ToString(),

                                    
                                });
                            }
                        }
                        foreach (var item in itemsModels)
                        {
                            ItemsSearch input = new ItemsSearch();
                            input.SearchTerm = item.ItemName.Trim().Substring(0,12);
                            //input1.SearchTerm = item.SupplierName;
                            var conditionitems = _itemsService.GetAllItems(input).Result.Items.Count();
                            if(conditionitems == 0 || conditionitems > 1 ) {
                                throw new UserFriendlyException("Mã hàng hóa " + input.SearchTerm + " chưa tồn tại trong hệ thống \n Vui lòng kiểm tra lại mã hàng hóa");
                            }
                            else
                            {
                                item.ItemId = _itemsService.GetAllItems(input).Result.Items.Select(x => x.Id).First();
                            }
                            var condition = _supplierAppService.GetAllSupplier().Result.Items.Where(x => item.SupplierName.Contains(x.Name)).ToList();
                            if (condition.Count() == 0)
                            {
                                throw new UserFriendlyException("Nhà Cung cấp " + item.SupplierName + "không có trong hệ thống \n Vui lòng kiểm tra lại tên nhà cung cấp");
                            }
                            else
                            {
                                item.SupplierName = _supplierAppService.GetAllSupplier().Result.Items.Where(x => item.SupplierName.Contains(x.Name)).Select(x => x.Name).First();
                                item.SupplierId = (int)_supplierAppService.GetAllSupplier().Result.Items.Where(x => item.SupplierName.Contains(x.Name)).Select(x => x.Id).First();
                            }
                        }
                    }
                    foreach (var item in itemsModels)
                    {
                        await _quotesService.Create(item);
                    }
                }
                return itemsModels;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocument(QuoteViewModel model)
        {
            try
            {
                //code Hà thêm
                string webRootPath = _iWebHostEnvironment.WebRootPath;
                string contentRootPath = _iWebHostEnvironment.ContentRootPath;

                model.FileUpload = Request.Form.Files.ToList();
                string path = "";

                FileInfo fileInfo = new FileInfo(model.FileUpload[0].FileName);
                var filePath = Path.Combine(Path.GetFileName(fileInfo.Name.ToString().Replace(fileInfo.Extension, "")) + Clock.Now.ToString("yyyyMMddhhmmss") + fileInfo.Extension);

                string fullFilePath;
                using (var stream = model.FileUpload[0].OpenReadStream())
                {
                    fullFilePath = await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(PersonalProfileCoreConsts.Quotation + @"//" + filePath, stream));

                }
                path = webRootPath + Path.DirectorySeparatorChar.ToString() + "templateEmail" + Path.DirectorySeparatorChar.ToString() + "sendMail.html";
                var pathImg = webRootPath + Path.DirectorySeparatorChar.ToString() + "templateEmail" + Path.DirectorySeparatorChar.ToString() + "Logo_Text.png";
                path = System.IO.File.ReadAllText(path);

                    var file = Request.Form.Files.ToList();
                   // path = path.Replace("{{TenNguoiNhan}}", model.SupplierName);
                   // path = path.Replace("{{TenCongViec}}", model.Title);
                    path = path.Replace("{{ImgSrc}}", pathImg);
                    path = path.Replace("{{TenNCC}}", model.SupplierName);

                    //await _sendMailService.SendEmailAsync(model.SupplierEmail, model.Title, path.ToString(), fullFilePath);




              //  }
                return Json(new AjaxResponse(new { data = model.Title }));

                //  return View("Index.cshtml");

            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }



        public async Task<PartialViewResult> CreateItems(int? WareHouseId)
        {
           
            return PartialView("_CreateModal");
        }



    }

}
