using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace bbk.netcore.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class DashboardController : netcoreControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
