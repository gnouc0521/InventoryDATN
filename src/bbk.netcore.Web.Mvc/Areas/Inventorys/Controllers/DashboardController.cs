using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
  [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class DashboardController : netcoreControllerBase
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _role;
       
        public DashboardController(
            UserManager userManager,  
            RoleManager role)

        {
            _userManager = userManager;
            _role = role;
        }
        
        public ActionResult Index()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            var roles = _userManager.GetRolesAsync(user);
            string rolename = _role.FindByNameAsync(roles.Result[0].ToString()).Result.DisplayName;
            ViewBag.name = rolename;
            ViewBag.userId = user.Id;
            return View();
        }
    }
}
