using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using bbk.netcore.Controllers;
using Microsoft.Extensions.Configuration;

namespace bbk.netcore.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : netcoreControllerBase
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ActionResult Index()
        {
            return Redirect(GetAppHomeUrl());
        }

        public string GetAppHomeUrl()
        {
            if (!string.IsNullOrEmpty(_configuration.GetSection("TenantSettings:HomePage").Value))
            {
                return _configuration.GetSection("TenantSettings:HomePage").Value;
            }
            else
                return Url.Action("Index", "Dashboard", new { area = "App" });

            //get config data
            string DataDirectory = _configuration.GetSection("TenantSettings:DataDir").Value;
        }
    }
}
