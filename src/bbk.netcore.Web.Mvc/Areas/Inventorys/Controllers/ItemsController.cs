using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.UI;
using Abp.Web.Models;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Inventorys;
using bbk.netcore.mdl.OMS.Application.Itemses;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Producers;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Application.Ruleses;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Application.Units;
using bbk.netcore.mdl.OMS.Application.WareHouses;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.Web.Areas.Inventorys.Models.Items;
using bbk.netcore.Web.Areas.Inventorys.Models.WarehouseLocationItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ItemsController : netcoreControllerBase
    {
        private readonly IRulesService _rulesService;
        private readonly IProducerAppService _producerAppService;
        private readonly ISupplierAppService _supplierAppService;
        private readonly IUnitService _unitService;
        private readonly IWareHouseAppService _wareHouseAppService;
        private readonly IItemsService _itemsAppService;
        private readonly IUserAppService _userAppService;
        private readonly IInventoryService _inventoryService;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        public ItemsController(IProducerAppService producerAppService,
                                ISupplierAppService supplierAppService,
                                IRulesService rulesService,
                                IUnitService unitService,
                                IWareHouseAppService wareHouseAppService,
                                IItemsService itemsService,
                                IUserAppService userAppService,
                                IInventoryService inventoryService,
                                IFileSystemBlobProvider fileSystemBlobProvider)
        {
            _rulesService = rulesService;
            _producerAppService = producerAppService;
            _supplierAppService = supplierAppService;
            _unitService = unitService;
            _wareHouseAppService = wareHouseAppService;
            _itemsAppService = itemsService;
            _userAppService = userAppService;
            _inventoryService = inventoryService;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }

        public async Task<ActionResult> Index(long? WareHouseId)
        {
            var warehouse = await _wareHouseAppService.GetAsync(new EntityDto<long>(WareHouseId.Value));

            if (WareHouseId.HasValue)
            {
                ViewBag.WareHouseId = WareHouseId.Value;

            }
            else
            {
                ViewBag.WareHouseId = 0;
            }
            ItemsModel itemsModel = new ItemsModel
            {
                WarehouseList = warehouse
            };
            return View(itemsModel);
        }
        public async Task<ActionResult> AllItems()
        {

            return View();
        }
        public async Task<ActionResult> DetailItem1(long Id)
        {
            var dto = await _itemsAppService.GetAsync(new EntityDto<long>(Id));
            var rulesCategory = _rulesService.GetAllCategory().Result.Where(x => x.ItemValue == dto.CategoryCode).ToList();
            var rulesGroup = _rulesService.GetAllGroup().Result.Where(x => x.ItemValue == dto.GroupCode).ToList();
            var rulesKind = _rulesService.GetAllKind().Result.Where(x => x.ItemValue == dto.KindCode).ToList();
            var producerList = _producerAppService.GetProducerList().Result.Where(x => x.Code == dto.ProducerCode).ToList();
            var supplierList = _supplierAppService.GetSupplierList().Result.Where(x => x.Code == dto.SupplierCode).ToList();
            var unitList = _unitService.GetUnitListDtos().Result.Where(x => x.Name == dto.Name).ToList();
            GetUsersInput getUsersInput = new GetUsersInput();
            getUsersInput.MaxResultCount = 500;
            var user = _userAppService.GetUsers(getUsersInput).Result.Items.Where(x => x.Id == dto.CreatorUserId).ToList();
            ViewBag.UserName = user[0].Surname + " " + user[0].Name;
            ItemsModel itemsModel = new ItemsModel
            {
                Items = dto,
                rulesCategory = rulesCategory,
                rulesGroup = rulesGroup,
                rulesKind = rulesKind,
                Producers = producerList,
                Suppliers = supplierList,
                unitListDtos = unitList,
            };
            return View("DetailItem1", itemsModel);
        }
        public async Task<PartialViewResult> CreateItems(int? WareHouseId)
        {
            var rulesCategory = await _rulesService.GetAllCategory();
            var rulesGroup = await _rulesService.GetAllGroup();
            var rulesKind = await _rulesService.GetAllKind();
            var producerList = await _producerAppService.GetProducerList();
            var supplierList = await _supplierAppService.GetSupplierList();
            var unitList = await _unitService.GetUnitListDtos();
            if (WareHouseId.HasValue)
            {
                ViewBag.WareHouseId = WareHouseId.Value;

            }
            else
            {
                ViewBag.WareHouseId = 0;
            }
            ItemsModel itemsModel = new ItemsModel
            {

                rulesCategory = rulesCategory,
                rulesGroup = rulesGroup,
                rulesKind = rulesKind,
                Producers = producerList,
                Suppliers = supplierList,
                unitListDtos = unitList
            };

            return PartialView("_CreateModal", itemsModel);
        }
        public async Task<IActionResult> EditItemsModal(long Id, int? WareHouseId)
        {
            var rulesCategory = await _rulesService.GetAllCategory();
            var rulesGroup = await _rulesService.GetAllGroup();
            var rulesKind = await _rulesService.GetAllKind();
            var producerList = await _producerAppService.GetProducerList();
            var supplierList = await _supplierAppService.GetSupplierList();
            var unitList = await _unitService.GetUnitListDtos();
            var dto = await _itemsAppService.GetAsync(new EntityDto<long>(Id));
            //var inventory = await _inventoryService.GetAsync(new EntityDto<long>(Id));

            if (WareHouseId.HasValue)
            {
                ViewBag.WareHouseId = WareHouseId.Value;

            }
            else
            {
                ViewBag.WareHouseId = 0;
            }
            ItemsModel itemsModel = new ItemsModel
            {
                Items = dto,
                rulesCategory = rulesCategory,
                rulesGroup = rulesGroup,
                rulesKind = rulesKind,
                Producers = producerList,
                Suppliers = supplierList,
                unitListDtos = unitList,
                // inventoryListDto = inventory,
            };

            return PartialView("_EditModal", itemsModel);
        }

        public async Task<ActionResult> DetailItemSearch(long Id, long? WareHouseId)
        {
            var inventory = await _inventoryService.GetAsync(new EntityDto<long>(Id));
            var dto = await _itemsAppService.GetAsync(new EntityDto<long>(inventory.ItemId));
            var rulesCategory = _rulesService.GetAllCategory().Result.Where(x => x.ItemValue == dto.CategoryCode).ToList();
            var rulesGroup = _rulesService.GetAllGroup().Result.Where(x => x.ItemValue == dto.GroupCode).ToList();
            var rulesKind = _rulesService.GetAllKind().Result.Where(x => x.ItemValue == dto.KindCode).ToList();
            var producerList = _producerAppService.GetProducerList().Result.Where(x => x.Code == dto.ProducerCode).ToList();
            var supplierList = _supplierAppService.GetSupplierList().Result.Where(x => x.Code == dto.SupplierCode).ToList();
            var unitList = _unitService.GetUnitListDtos().Result.Where(x => x.Name == dto.Name).ToList();
            GetUsersInput getUsersInput = new GetUsersInput();
            getUsersInput.MaxResultCount = 500;
            var user = _userAppService.GetUsers(getUsersInput).Result.Items.Where(x => x.Id == dto.CreatorUserId).ToList();
            ViewBag.UserName = user[0].Surname + " " + user[0].Name;
            var warehouse = await _wareHouseAppService.GetAsync(new EntityDto<long>(WareHouseId.Value));
            if (WareHouseId.HasValue)
            {
                ViewBag.WareHouseId = WareHouseId.Value;
            }
            else
            {
                ViewBag.WareHouseId = 0;
            }
            ItemsModel itemsModel = new ItemsModel
            {
                Items = dto,
                rulesCategory = rulesCategory,
                rulesGroup = rulesGroup,
                rulesKind = rulesKind,
                Producers = producerList,
                Suppliers = supplierList,
                unitListDtos = unitList,
                WarehouseList = warehouse,
                inventoryListDto = inventory
            };
            return View(itemsModel);
        }
        public async Task<ActionResult> Detail(long Id, long? WareHouseId)
        {
            var inventory = await _inventoryService.GetAsync(new EntityDto<long>(Id));
            var dto = await _itemsAppService.GetAsync(new EntityDto<long>(inventory.ItemId));
            var rulesCategory = _rulesService.GetAllCategory().Result.Where(x => x.ItemValue == dto.CategoryCode).ToList();
            var rulesGroup = _rulesService.GetAllGroup().Result.Where(x => x.ItemValue == dto.GroupCode).ToList();
            var rulesKind = _rulesService.GetAllKind().Result.Where(x => x.ItemValue == dto.KindCode).ToList();
            var producerList = _producerAppService.GetProducerList().Result.Where(x => x.Code == dto.ProducerCode).ToList();
            var supplierList = _supplierAppService.GetSupplierList().Result.Where(x => x.Code == dto.SupplierCode).ToList();
            var unitList = _unitService.GetUnitListDtos().Result.Where(x => x.Name == dto.Name).ToList();
            GetUsersInput getUsersInput = new GetUsersInput();
            getUsersInput.MaxResultCount = 500;
            var user = _userAppService.GetUsers(getUsersInput).Result.Items.Where(x => x.Id == dto.CreatorUserId).ToList();
            ViewBag.UserName = user[0].Surname + " " + user[0].Name;
            var warehouse = await _wareHouseAppService.GetAsync(new EntityDto<long>(WareHouseId.Value));
            if (WareHouseId.HasValue)
            {
                ViewBag.WareHouseId = WareHouseId.Value;
            }
            else
            {
                ViewBag.WareHouseId = 0;
            }
            ItemsModel itemsModel = new ItemsModel
            {
                Items = dto,
                rulesCategory = rulesCategory,
                rulesGroup = rulesGroup,
                rulesKind = rulesKind,
                Producers = producerList,
                Suppliers = supplierList,
                unitListDtos = unitList,
                WarehouseList = warehouse,
                inventoryListDto = inventory
            };
            return View(itemsModel);
        }
        [HttpPost]
        public async Task<JsonResult> OverView(int Id, long WarehouseId)
        {
            return Json(new AjaxResponse(new { id = Id, warehouseId = WarehouseId }));
        }

        [HttpPost]
        public async Task<JsonResult> OverViewByCode(string ItemCode)
        {
            return Json(new AjaxResponse(new { itemcode = ItemCode }));
        }

        public async Task<ActionResult> DetailItemByCode(string ItemCode, long? WareHouseId)
        {

            var dto = await _itemsAppService.GetItemByCode(ItemCode);
            var rulesCategory = _rulesService.GetAllCategory().Result.Where(x => x.ItemValue == dto.CategoryCode).ToList();
            var rulesGroup = _rulesService.GetAllGroup().Result.Where(x => x.ItemValue == dto.GroupCode).ToList();
            var rulesKind = _rulesService.GetAllKind().Result.Where(x => x.ItemValue == dto.KindCode).ToList();
            var producerList = _producerAppService.GetProducerList().Result.Where(x => x.Code == dto.ProducerCode).ToList();
            var supplierList = _supplierAppService.GetSupplierList().Result.Where(x => x.Code == dto.SupplierCode).ToList();
            var unitList = _unitService.GetUnitListDtos().Result.Where(x => x.Name == dto.Name).ToList();
            GetUsersInput getUsersInput = new GetUsersInput();
            getUsersInput.MaxResultCount = 500;
            var user = _userAppService.GetUsers(getUsersInput).Result.Items.Where(x => x.Id == dto.CreatorUserId).ToList();
            ViewBag.UserName = user[0].Surname + " " + user[0].Name;
            //var warehouse = await _wareHouseAppService.GetAsync(new EntityDto<long>(WareHouseId.Value));
            ////var inventory = await _inventoryService.GetAsync(new EntityDto<long>(Id));
            //if (WareHouseId.HasValue)
            //{
            //    ViewBag.WareHouseId = WareHouseId.Value;
            //}
            //else
            //{
            //    ViewBag.WareHouseId = 0;
            //}
            ItemsModel itemsModel = new ItemsModel
            {
                Items = dto,
                rulesCategory = rulesCategory,
                rulesGroup = rulesGroup,
                rulesKind = rulesKind,
                Producers = producerList,
                Suppliers = supplierList,
                unitListDtos = unitList,
                //WarehouseList = warehouse,
                //inventoryListDto = inventory
            };
            return View(itemsModel);
        }

        //Code Ha
        public async Task<ActionResult> SearchItem()
        {
            var producerList = await _producerAppService.GetProducerList();
            var supplierList = await _supplierAppService.GetSupplierList();
            var warehouseList = await _wareHouseAppService.GetWarehouseList();
            WarehouseLocationItemModel modal = new WarehouseLocationItemModel
            {
                ListProducer = producerList,
                ListSupplier = supplierList,
                ListWarehouse = warehouseList,
            };
            return View(modal);
        }

        /// <summary>
        /// Downloadfile Templates import Items
        /// </summary>
        /// <returns></returns>
        public IActionResult Downloadfile()
        {
            var memory = DownloadSinghFile("ImportItems.xlsx", "wwwroot\\data\\tenants\\1\\Templates");
            return File(memory.ToArray(), "application/vnd.ms-excel", "ImportItems.xlsx");
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
       

        public async Task<List<ItemsListDto>> ImportExcel(IFormFile file)
        {
            try
            {
                List<ItemsListDto> itemsModels = new List<ItemsListDto>();
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
                            for (int i = 5; i < rows +4; i++)
                            {

                                itemsModels.Add(new ItemsListDto
                                {
                                    Name = (workSheet.Cells[i, 2].Value ?? string.Empty).ToString(),
                                    Description = (workSheet.Cells[i, 3].Value ?? string.Empty).ToString(),
                                    ProducerCode = (workSheet.Cells[i, 4].Value ?? string.Empty).ToString(),
                                    SymbolCode = (workSheet.Cells[i, 5].Value ?? string.Empty).ToString(),
                                    CategoryCode = (workSheet.Cells[i, 6].Value ?? string.Empty).ToString(),
                                    GroupCode = (workSheet.Cells[i, 7].Value ?? string.Empty).ToString(),
                                    KindCode = (workSheet.Cells[i, 8].Value ?? string.Empty).ToString(),

                                    });
                            }

                        }
                      
                    }
                    foreach (var item in itemsModels)
                    {
                        RulesSearch input = new RulesSearch();
                        GetProducerInput getProducerInput = new GetProducerInput();
                        item.KindCode = _rulesService.GetAll(input).Result.Items.Where(x => x.ItemText.Equals(item.KindCode) && x.ItemKey.Equals("CHL")).Select(x=>x.ItemValue).FirstOrDefault();
                        item.CategoryCode = _rulesService.GetAll(input).Result.Items.Where(x => x.ItemText.Equals(item.CategoryCode) && x.ItemKey.Equals("NGH")).Select(x => x.ItemValue).FirstOrDefault();
                        item.GroupCode = _rulesService.GetAll(input).Result.Items.Where(x => x.ItemText.Equals(item.GroupCode) && x.ItemKey.Equals("NHH")).Select(x => x.ItemValue).FirstOrDefault();
                        item.ProducerCode = _producerAppService.GetAll(getProducerInput).Result.Items.Where(x=>x.Name.Equals(item.ProducerCode)).Select(x=>x.Code).FirstOrDefault();
                    }
                    foreach (var item in itemsModels)
                    {
                       await _itemsAppService.Create(item);
                    }
                }
                return itemsModels;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
            
        }

      
    }
}

