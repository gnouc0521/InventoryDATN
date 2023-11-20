using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.UI;
using Abp.Web.Models;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequests;
using bbk.netcore.mdl.OMS.Application.Itemses;
using bbk.netcore.mdl.OMS.Application.Subsidiaries;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Application.Units;
using bbk.netcore.mdl.OMS.Application.WareHouses;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.Web.Areas.Inventorys.Models.ImportRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
  public class ImportRequestController : netcoreControllerBase
  {
    private readonly IImportRequestAppService _importRequestAppService;
    private readonly ISupplierAppService _supplierAppService;
    private readonly IWareHouseAppService _wareHouseAppService;
    private readonly IItemsService _itemAppService;
    private readonly UserManager _userManager;
    private readonly IUnitService _unitService;
    private readonly ISubsidiaryService _subsidiaryService;
    public ImportRequestController(IImportRequestAppService importRequestAppService,
                                   ISupplierAppService supplierAppService,
                                   IWareHouseAppService wareHouseAppService,
                                   IItemsService itemAppService,
                                   UserManager userManager,
                                   IUnitService unitService,
                                    ISubsidiaryService subsidiaryService
                                  )
    {
      _importRequestAppService = importRequestAppService;
      _supplierAppService = supplierAppService;
      _wareHouseAppService = wareHouseAppService;
      _userManager = userManager;
      _itemAppService = itemAppService;
      _unitService = unitService;
      _subsidiaryService = subsidiaryService;
    }


    /// <summary>
    /// xuất nhập kho điều chuyển
    /// </summary>
    /// <returns></returns>


    public async Task<IActionResult> ViewUpdateIMP()
    {
      GetWarehouseInput getWarehouseInput = new GetWarehouseInput();
      var dto = await _wareHouseAppService.GetAll(getWarehouseInput);
      List<SelectListItem> listItems = new List<SelectListItem>();
      foreach (var role in _userManager.Users)
        listItems.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.FullName });
      ViewBag.Name = listItems;
      IndexViewModel model = new IndexViewModel
      {
        WarehouseList = dto.Items.ToList()
      };
      return View(model);
    }


    //public async Task<IActionResult> CreateImportRequest(long Id)
    //{
    //    var transferList = await _iTransferAppService.GetAsync(new EntityDto<long>(Id));
    //    var transDferList = _iTransferDetailAppService.GetAllItem().Result.Items.FirstOrDefault(x => x.TransferId == transferList.Id);
    //    var warehouseList = await _wareHouseAppService.GetWarehouseList();
    //    var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
    //    ImportRequestViewModel model = new ImportRequestViewModel
    //    {
    //        WarehouseList = warehouseList,
    //        CreatedBy = User.FullName,
    //        TransferId = Id,
    //        TransferNote = transferList.TransferNote,
    //        TransferCode = transferList.TransferCode,
    //        IdWarehouseExport = transferList.IdWarehouseExport,
    //        Status2 = transferList.Status,
    //        BrowsingTime = transferList.BrowsingTime,
    //        IdWarehouseReceiving = transDferList.IdWarehouseReceiving,

    //    };
    //    return PartialView("_CreateModal", model);
    //}


    //public async Task<IActionResult> EditImportRequestModal(int Id)
    //{
    //    var dto = await _importRequestAppService.GetAsync(new EntityDto(Id));
    //    var warehouseList = await _wareHouseAppService.GetWarehouseList();
    //    var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);

    //    //var transferList = await _iTransferAppService.GetAsync(new EntityDto<long>(dto.TransferId));

    //    //var YCNKList = await _importRequestSubidiaryService.GetAsync(new EntityDto((int)dto.ImportRequestSubsidiaryId));


    //    ImportRequestViewModel model = new ImportRequestViewModel
    //    {
    //        WarehouseList = warehouseList,
    //        CreatedBy = User.FullName,
    //        Code = dto.Code,
    //        WarehouseDestinationId = dto.WarehouseDestinationId,
    //        RequestDate = dto.RequestDate,
    //        impRequests = dto,
    //        SubsidiaryId = dto.SubsidiaryId,
    //       // ImportRequestSubsidiary = yy,
    //        //TransferNote = transferList.TransferNote,
    //        TransferId = dto.TransferId,
    //        ShipperName = dto.ShipperName,
    //        ShipperPhone = dto.ShipperPhone,
    //      //  ImportRequestSubsidiaryId = dto.ImportRequestSubsidiaryId
    //    };

    //    return PartialView("_EditModal", model);
    //}

    public async Task<IActionResult> ViewDetails(int Id)
    {
      var dto = await _importRequestAppService.GetAsync(new EntityDto(Id));
      var warehouseList = _wareHouseAppService.GetWarehouseList().Result.Where(x => x.Id == dto.WarehouseDestinationId).FirstOrDefault();
      var UserCreate = _userManager.Users.Where(x => x.Id == dto.CreatorUserId).FirstOrDefault();
      ImportRequestViewModel model = new ImportRequestViewModel
      {
        NameWareHouse = warehouseList.Name,
        CreatedBy = UserCreate.FullName,
        Code = dto.Code,
        WarehouseDestinationId = dto.WarehouseDestinationId,
        RequestDate = dto.RequestDate,
        ImportStatus = dto.ImportStatus,
        ShipperName = dto.ShipperName,
        ShipperPhone = dto.ShipperPhone,
        impRequests = dto
      };
      return View("ViewDetails", model);
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    /// <summary>
    /// XUẤT NHẬP KHO THƯỜNG
    /// </summary>
    /// <returns></returns>

    public async Task<IActionResult> ImportRequest()
    {
      GetWarehouseInput getWarehouseInput = new GetWarehouseInput();
      var dto = await _wareHouseAppService.GetAll(getWarehouseInput);
      List<SelectListItem> listItems = new List<SelectListItem>();
      foreach (var role in _userManager.Users)
        listItems.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.FullName });
      ViewBag.Name = listItems;
      IndexViewModel model = new IndexViewModel
      {
        WarehouseList = dto.Items.ToList()
      };
      return View(model);
    }


    public async Task<IActionResult> CreateIMP()
    {
      var warehouseList = await _wareHouseAppService.GetWarehouseList();
      var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
      ImportRequestViewModel model = new ImportRequestViewModel
      {
        WarehouseList = warehouseList,
        CreatedBy = User.FullName,
      };
      return PartialView("_CreateImp", model);
    }




    public async Task<IActionResult> EditImp(int Id)
    {
      var dto = await _importRequestAppService.GetAsync(new EntityDto(Id));
      var supplierList = await _supplierAppService.GetSupplierList();
      var warehouseList = await _wareHouseAppService.GetWarehouseList();
      var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
      ImportRequestViewModel model = new ImportRequestViewModel
      {
        WarehouseList = warehouseList,
        Suppliers = supplierList,
        CreatedBy = User.FullName,
        Code = dto.Code,
        WarehouseDestinationId = dto.WarehouseDestinationId,
        RequestDate = dto.RequestDate,
        ShipperName = dto.ShipperName,
        ShipperPhone = dto.ShipperPhone,
        Id = Id,
      };

      return PartialView("_EditImp", model);
    }



    public async Task<IActionResult> ViewUpdateDetails(int Id)
    {
      var dto = await _importRequestAppService.GetAsync(new EntityDto(Id));
      var warehouseList = _wareHouseAppService.GetWarehouseList().Result.Where(x => x.Id == dto.WarehouseDestinationId).FirstOrDefault();
      var UserCreate = _userManager.Users.Where(x => x.Id == dto.CreatorUserId).FirstOrDefault();
      ImportRequestViewModel model = new ImportRequestViewModel
      {
        NameWareHouse = warehouseList.Name,
        CreatedBy = UserCreate.FullName,
        Code = dto.Code,
        WarehouseDestinationId = dto.WarehouseDestinationId,
        RequestDate = dto.RequestDate,
        ImportStatus = dto.ImportStatus,
        ShipperName = dto.ShipperName,
        ShipperPhone = dto.ShipperPhone,
        impRequests = dto
      };
      return View("ViewUpdateDetails", model);
    }

    public async Task<IActionResult> Print(int Id)
    {
      var dto = await _importRequestAppService.GetAsync(new EntityDto(Id));
      var warehouseList = _wareHouseAppService.GetWarehouseList().Result.Where(x => x.Id == dto.WarehouseDestinationId).FirstOrDefault();
      var UserCreate = _userManager.Users.Where(x => x.Id == dto.CreatorUserId).FirstOrDefault();
      ImportRequestViewModel model = new ImportRequestViewModel
      {
        NameWareHouse = warehouseList.Name,
        CreatedBy = UserCreate.FullName,
        Code = dto.Code,
        WarehouseDestinationId = dto.WarehouseDestinationId,
        RequestDate = dto.RequestDate,
        ShipperName = dto.ShipperName,
        ShipperPhone = dto.ShipperPhone,
        impRequests = dto,
        DiaChiKho = warehouseList.Number,
      };
      return View("Print", model);
    }



    /// <summary>
    /// Nhap kho tu YCNK
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public async Task<IActionResult> ImportRequestYCNK()
    {
      GetWarehouseInput getWarehouseInput = new GetWarehouseInput();
      var dto = await _wareHouseAppService.GetAll(getWarehouseInput);
      List<SelectListItem> listItems = new List<SelectListItem>();
      foreach (var role in _userManager.Users)
        listItems.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.FullName });
      ViewBag.Name = listItems;
      IndexViewModel model = new IndexViewModel
      {
        WarehouseList = dto.Items.ToList()
      };
      return View(model);
    }


    //public async Task<IActionResult> CreateImportYCNK(long Id)
    //{
    //    var YCNKList = await _importRequestSubidiaryService.GetAsync(new EntityDto(((int)Id)));
    //    var YCNKdList = _importRequestDetailSubidiaryService.GetAllItem().Result.Items.FirstOrDefault(x => x.ImportRequestSubsidiaryId == YCNKList.Id);
    //    var warehouseList = await _wareHouseAppService.GetWarehouseList();
    //    var User = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
    //    var supplierList = await _supplierAppService.GetSupplierList();
    //    ImportRequestViewModel model = new ImportRequestViewModel
    //    {
    //        WarehouseList = warehouseList,
    //        CreatedBy = User.FullName,
    //        ImportRequestSubsidiary = YCNKList,
    //        Suppliers = supplierList,
    //    };
    //    return PartialView("_CreateImportYCNK", model);
    //}


    [HttpPost]
    public async Task<JsonResult> OverView(int Id)
    {
      return Json(new AjaxResponse(new { id = Id }));
    }

    public IActionResult Downloadfile()
    {
      var memory = DownloadSinghFile("Phieunhap.xlsx", "wwwroot\\data\\tenants\\1\\Templates");
      return File(memory.ToArray(), "application/vnd.ms-excel", "Phieunhap.xlsx");
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

    public async Task<ImportRequestViewModel> ImportExcel(IFormFile file)
    {
      try
      {
        List<ImportRequestDetailListDto> ImportRequestDetailListDto = new List<ImportRequestDetailListDto>();
        ImportRequestViewModel importRequestViewModel = new ImportRequestViewModel();
        using (var stream = new MemoryStream())
        {

          await file.CopyToAsync(stream);
          using (var package = new ExcelPackage(stream))
          {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

            // get number of rows and columns in the sheet
            int rows = workSheet.Dimension.Rows; // 20
            int columns = workSheet.Dimension.Columns; // 7
            string name = "Người vận chuyển : ";
            string phone = "SDT người vận chuyển:";
            string inventory = "Kho nhập:";
            string remark = "Lý do nhập kho:";
            importRequestViewModel.ShipperName = (workSheet.Cells[2, 1].Value ?? string.Empty).ToString().Replace("Người vận chuyển : ", " ").Trim();
            importRequestViewModel.ShipperPhone = (workSheet.Cells[3, 1].Value ?? string.Empty).ToString().Replace("SDT người vận chuyển:", " ").Trim();
            importRequestViewModel.NameWareHouse = (workSheet.Cells[4, 1].Value ?? string.Empty).ToString().Replace(inventory, "").Trim();
            importRequestViewModel.Remark = (workSheet.Cells[5, 1].Value ?? string.Empty).ToString().Replace(remark, "").Trim();
            if (workSheet != null)
            {
              for (int i = 10; i <= 10 + (rows - 9); i++)
              {
                ImportRequestDetailListDto.Add(new ImportRequestDetailListDto
                {
                  CodeItem = (workSheet.Cells[i, 2].Value ?? string.Empty).ToString(),
                  UnitName = (workSheet.Cells[i, 6].Value ?? string.Empty).ToString(),
                  ImportPrice = (decimal)(workSheet.Cells[i, 3].Value ?? string.Empty),
                  Quantity = (int)(workSheet.Cells[i, 3].Value ?? string.Empty)
                });
              }
            }
            importRequestViewModel.ImportRequestDetailListDto = ImportRequestDetailListDto;
          }
        }
        return importRequestViewModel;
      }
      catch (Exception ex)
      {

        throw new UserFriendlyException(ex.Message);
      }

    }
  }
}
