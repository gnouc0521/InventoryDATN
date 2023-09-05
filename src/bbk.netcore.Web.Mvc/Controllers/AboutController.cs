using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Controllers;

namespace bbk.netcore.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : netcoreControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
