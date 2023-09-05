using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace bbk.netcore.mdl.Inventorys.Web.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class OrgAndPayrollsController : AbpController
    {

        public OrgAndPayrollsController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
