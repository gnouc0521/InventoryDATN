using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using bbk.netcore.Web.Areas.App.Startup;
using bbk.netcore.Web.Views;
using bbk.netcore.mdl.PersonalProfile.Web.Navigation;

namespace bbk.netcore.Web.Areas.App.Views.Shared.Components.AppNavigation
{
    public class AppNavigationViewComponent : netcoreViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;

        public AppNavigationViewComponent(
            IUserNavigationManager userNavigationManager,
            IAbpSession abpSession)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AppNavigationViewModel
            {
                //MainMenu = await _userNavigationManager.GetMenuAsync("MainMenu", _abpSession.ToUserIdentifier())
                AppMenu = await _userNavigationManager.GetMenuAsync(AppNavigationProvider.MenuName, _abpSession.ToUserIdentifier()),
                PersonalProfileMenu = await _userNavigationManager.GetMenuAsync(InventorysNavigationProvider.MenuName, _abpSession.ToUserIdentifier()),
            };

            return View(model);
        }
    }
}
