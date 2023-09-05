using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.WorkGroups;
using bbk.netcore.Web.Areas.Inventorys.Models.WorkGroups;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class WorkGroupController : netcoreControllerBase
    {
        private readonly IWorkGroupAppService _workGroupAppService;
        public WorkGroupController(IWorkGroupAppService workGroupAppService)
        {
            _workGroupAppService = workGroupAppService;
        }

        public async Task<ActionResult> Index()
        {
            var listWorkGroup = _workGroupAppService.GetAllList();
            IndexViewWorkGroupModel model = new IndexViewWorkGroupModel()
            {
                workGroupListDtos = listWorkGroup.Result.Items,
            }; 
            return View(model);

        }

        public PartialViewResult CreateWorkRoot()
        {
            return PartialView("_CreateWorkRootModal");
        }

        public async Task<ActionResult> CreateWorkItems(int Id)
        {
            var dto = await _workGroupAppService.GetAsync(new EntityDto(Id));
            return PartialView("_CreateWorkItemsModal", dto.Items[0]);
        }

        public async Task<IActionResult> EditWorkGroup(int Id)
        {
            var dto = await _workGroupAppService.GetAsync(new EntityDto(Id));
            return PartialView("_EditWorkGroupModal", dto.Items[0]);
        }
    }
}
