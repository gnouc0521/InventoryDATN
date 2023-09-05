using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class ReportWorksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
