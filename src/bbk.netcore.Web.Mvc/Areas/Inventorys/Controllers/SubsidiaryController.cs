using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using bbk.netcore.mdl.OMS.Application.Subsidiaries;
using bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class SubsidiaryController : netcoreControllerBase
    {
        private readonly ISubsidiaryService _iSubsidiaryService;

        public SubsidiaryController(ISubsidiaryService iSubsidiaryService)
        {
            _iSubsidiaryService= iSubsidiaryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> Create()
        {
            var province = await _iSubsidiaryService.GetAddress("province.json", null);
            SubsidiaryCreateDto model = new SubsidiaryCreateDto 
            { 
                provinces = province,

            
            };
            
            return PartialView("_CreateModal", model);
        }

        public async Task<PartialViewResult> Update(int Id)
        {
            var dto = await _iSubsidiaryService.GetAsync(new EntityDto(Id));
            var province = await _iSubsidiaryService.GetAddress("province.json", null);
            SubsidiaryListDto model = new SubsidiaryListDto
            {
                Id = Id,
                NameCompany = dto.NameCompany,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                EmailAddress= dto.EmailAddress,
                CityId= dto.CityId,
                DistrictId= dto.DistrictId,
                WardsId= dto.WardsId,
                provinces = province,
                
            };
            return PartialView("_EditModal", model);
        }

        public async Task<IActionResult> ViewDetails(int Id)
        {
            var dto = await _iSubsidiaryService.GetAsync(new EntityDto(Id));
            var province = await _iSubsidiaryService.GetAddress("province.json", null);
            SubsidiaryListDto model = new SubsidiaryListDto
            {
                Id = Id,
                NameCompany = dto.NameCompany,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                EmailAddress = dto.EmailAddress,
                CityId = dto.CityId,
                DistrictId = dto.DistrictId,
                WardsId = dto.WardsId,
                provinces = province
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
