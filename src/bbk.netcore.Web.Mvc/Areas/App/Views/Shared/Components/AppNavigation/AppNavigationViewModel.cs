using Abp.Application.Navigation;

namespace bbk.netcore.Web.Areas.App.Views.Shared.Components.AppNavigation
{
    public class AppNavigationViewModel
    {
        public UserMenu MainMenu { get; set; }
        public UserMenu AppMenu { get; set; }
        public UserMenu PersonalProfileMenu { get; set; }
    }
}
