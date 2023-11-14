using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Producers;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
  [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ProducerController : netcoreControllerBase
    {
        private readonly IProducerAppService _producerAppService;

        public ProducerController(IProducerAppService producerAppService)
        {
            _producerAppService = producerAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateProducer()
        {
            var province = await _producerAppService.GetAddress("province.json", null);
            ProducerCreateDto model = new ProducerCreateDto { provinces = province };
            return PartialView("_CreateModal", model);
        }

        public async Task<IActionResult> EditProducerModal(int Id)
        {
            var dto = await _producerAppService.GetAsync(new EntityDto(Id));
            var province = await _producerAppService.GetAddress("province.json", null);
            ProducerListDto model = new ProducerListDto
            {
                provinces = province,
                Name = dto.Name,
                Code = dto.Code,
                Adress = dto.Adress,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Area = dto.Area,
                Bank = dto.Bank,
                Fax = dto.Fax,
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
            var dto = await _producerAppService.GetAsync(new EntityDto(Id));
            var province = await _producerAppService.GetAddress("province.json", null);
            ProducerListDto model = new ProducerListDto
            {
                provinces = province,
                Name = dto.Name,
                Code = dto.Code,
                Adress = dto.Adress,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Area = dto.Area,
                Bank = dto.Bank,
                Fax = dto.Fax,
                Id = Id,
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
