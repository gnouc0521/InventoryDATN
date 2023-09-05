using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Application.Units;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.Inventorys.Web.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class UnitsController : AbpController
    {
        private readonly IUnitService _unitService;

        public UnitsController(
            IUnitService unitService
            )
        {
            _unitService = unitService;
        }

        public async Task<ActionResult> Index()
        {
            
            return View();
        }
        public async Task<IActionResult> CreateUnits(int? id)
        {
            UnitListDto dto = new UnitListDto();
            if (id.HasValue)
            {
                var output = await _unitService.GetAsync(new EntityDto(id.Value));
                dto = output.MapTo(dto);
            }
            return PartialView("_CreateModal" , dto);
        }
        public async Task<IActionResult> EditUnitsModal(int Id)
        {
            var dto = await _unitService.GetAsync(new EntityDto(Id));
            return PartialView("_EditModal", dto);
        }

    }
}
