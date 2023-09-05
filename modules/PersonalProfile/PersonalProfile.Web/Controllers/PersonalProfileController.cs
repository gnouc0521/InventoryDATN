using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.Mvc.Controllers;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles;
using bbk.netcore.mdl.PersonalProfile.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Web.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class PersonalProfileController : AbpController
    {
        private readonly IPersonalProfileAppService _personalProfileAppService;

        public PersonalProfileController(IPersonalProfileAppService personalProfileAppService)
        {
            _personalProfileAppService = personalProfileAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile(int id)
        {
            var person = await _personalProfileAppService.Get(id);
            PersonalProfileViewModel result = new PersonalProfileViewModel
            {
                PersonProfile = person
            };
            return View(result);
        }

    }
}
