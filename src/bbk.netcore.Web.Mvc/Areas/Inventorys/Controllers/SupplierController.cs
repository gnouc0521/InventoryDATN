using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Producers;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class SupplierController : netcoreControllerBase
    {
        private readonly ISupplierAppService _supplierAppService;

        public SupplierController(ISupplierAppService supplierAppService)
        {
           _supplierAppService= supplierAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateSupplier()
        {
            var province = await _supplierAppService.GetAddress("province.json", null);
            SupplierListDto model = new SupplierListDto { provinces = province };
            return PartialView("_CreateModal",model);
        }

        public async Task<IActionResult> EditSupplierModal(int Id)
        {
            var dto = await _supplierAppService.GetAsync(new EntityDto(Id));
            var province = await _supplierAppService.GetAddress("province.json", null);
            SupplierListDto model = new SupplierListDto
            {
                provinces= province,
                Name = dto.Name,
                Code = dto.Code,
                Adress = dto.Adress,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Area = dto.Area,
                Bank = dto.Bank,
                Fax = dto.Fax,
                STK = dto. STK,
                Id = Id,
                Website = dto.Website,
                TaxCode = dto.TaxCode,
                NameRepresentative = dto.NameRepresentative,
                Remark = dto.Remark,
            };
            return PartialView("_EditModal", model);
        }

        public async Task<IActionResult> ViewDetails(int Id)
        {
            var dto = await _supplierAppService.GetAsync(new EntityDto(Id));
            SupplierListDto model = new SupplierListDto
            {
                Name = dto.Name,
                Code = dto.Code,
                Adress = dto.Adress,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Area = dto.Area,
                Bank = dto.Bank,
                Fax = dto.Fax,
                Id = Id,
                STK = dto.STK,
                Website = dto.Website,
                TaxCode = dto.TaxCode,
                NameRepresentative = dto.NameRepresentative,
                Remark = dto.Remark,
            };
            return View("ViewDetails", model);
        }

        [HttpPost]
        public async Task<JsonResult> OverView(int Id)
        {
            return Json(new AjaxResponse(new { id = Id }));
        }
    }
}
