using Abp.Application.Navigation;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Web.Navigation
{
    public class PersonalProfileNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu.AddItem(
                    new MenuItemDefinition(
                        "PersonalProfile_Management",
                        new FixedLocalizableString("QLCBNVManagement"),
                        url: "#",
                        icon: "fal fa-info-circle"
                        //requiresAuthentication: true
                    ).AddItem(
                        new MenuItemDefinition(
                            "PersonalProfile_Management",
                            L("PersonalProfile_Management"),
                            url: "PersonalProfile",
                            icon: "fas fa-users"
                        )
                    )
                )
            ;

        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, bbk.netcore.netcoreConsts.LocalizationSourceName);
        }

    }
}
