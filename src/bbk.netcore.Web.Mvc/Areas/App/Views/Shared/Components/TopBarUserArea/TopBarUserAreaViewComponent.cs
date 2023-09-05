using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Microsoft.AspNetCore.Mvc;
using bbk.netcore.Web.Views;
using bbk.netcore.Sessions;

namespace bbk.netcore.Web.Areas.App.Views.Shared.Components.TopBarUserArea
{
    public class TopBarUserAreaViewComponent : netcoreViewComponent
    {
        private readonly ISessionAppService _sessionAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public TopBarUserAreaViewComponent(
            ISessionAppService sessionAppService,
            IMultiTenancyConfig multiTenancyConfig)
        {
            _sessionAppService = sessionAppService;
            _multiTenancyConfig = multiTenancyConfig;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new TopBarUserAreaViewModel
            {
                LoginInformations = await _sessionAppService.GetCurrentLoginInformations(),
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
            };

            return View(model);
        }
    }
}
