using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using bbk.netcore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";
        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                // root "TenanciesGroup"
                .AddItem(new MenuItemDefinition(
                        "TenanciesGroup",
                        L("Tenancies"),
                        url: "#",
                        icon: "fal fa-info-circle",
                        requiresAuthentication: true,
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                    ).AddItem(
                        new MenuItemDefinition(
                            AppPageNames.Host.Dashboard,
                            L("TenancyDashboard"),
                            url: "TenancyDashboard",
                            icon: "fas fa-building",
                            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            AppPageNames.Host.Tenants,
                            L("Tenants"),
                            url: "Tenants",
                            icon: "fas fa-building",
                            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                        )
                    )
                    .AddItem(
                        new MenuItemDefinition(
                            AppPageNames.Host.Editions,
                            L("Editions"),
                            url: "Editions",
                            icon: "fas fa-building",
                            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                        )
                    )
                )
                // root "AdministrationGroup"
                .AddItem(
                    new MenuItemDefinition(
                        "AdministrationGroup",
                        //L("Administration"),
                        new FixedLocalizableString("Quản trị phần mềm"),
                        url: "#",
                        icon: "fal fa-info-circle",
                        requiresAuthentication: true
                    )
                    //.AddItem(
                    //    new MenuItemDefinition(
                    //        AppPageNames.AdminCommon.OrganizationUnits,
                    //        L("OrganizationUnits"),
                    //        url: "/App/OrganizationUnits",
                    //        icon: "fas fa-users",
                    //        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Users)
                    //    )
                    //)
                    .AddItem(
                        new MenuItemDefinition(
                            AppPageNames.AdminCommon.Roles,
                            L("Roles"),
                            url: "/App/Roles",
                            icon: "fas fa-theater-masks",
                            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Roles)
                        )
                    )
                    .AddItem(
                        new MenuItemDefinition(
                            AppPageNames.AdminCommon.Users,
                            new FixedLocalizableString("Người dùng"),
                            url: "/App/Users",
                            icon: "fas fa-users",
                            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Users)
                        )
                    )
                    .AddItem(
                        new MenuItemDefinition(
                            AppPageNames.AdminCommon.Users,
                            new FixedLocalizableString("Giao diện"),
                            url: "/Inventorys/Settings/Index",
                            icon: "fas fa-users",
                            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Users)
                        )
                    )
                    //.AddItem(
                    //    new MenuItemDefinition(
                    //        AppPageNames.AdminCommon.Users,
                    //        new FixedLocalizableString("Danh mục"),
                    //        url: "/PersonalProfile/Staffs/Category",
                    //        icon: "fas fa-users",
                    //        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Users)
                    //    )
                    //)
                )
                // root "PRM", features 'QLCB'; "CBNV TT 2.c", "
                //.AddItem(
                //    new MenuItemDefinition(
                //        "QLCBNVGroup",
                //        L("QLCBNVManagement"),
                //        url: "#",
                //        icon: "fal fa-info-circle",
                //        requiresAuthentication: true
                //    )
                //)
                //.AddItem(
                //    new MenuItemDefinition(
                //        "PersonalProfileMgrGroup",
                //        L("PersonalProfileMgrGroup"),
                //        url: "#",
                //        icon: "fal fa-info-circle",
                //        requiresAuthentication: true
                //    )
                //    //.AddItem(
                //    //    new MenuItemDefinition(
                //    //        "Pages_PPS_Staffs",
                //    //        L("Pages_PPS_Staffs"),
                //    //        url: "/PersonalProfile/Staffs",
                //    //        icon: "fal fa-info-circle",
                //    //        requiresAuthentication: true                            
                //    //    )
                //    //)
                //)
                // root "School", features 'School'; "students, pupils management", "
                //.AddItem(
                //    new MenuItemDefinition(
                //        "SchoolMgrGroup",
                //        L("SchoolInfomations"),
                //        url: "#",
                //        icon: "fal fa-info-circle",
                //        requiresAuthentication: true
                //    )
                //)
                //.AddItem(
                //    new MenuItemDefinition(
                //        "SchoolManagement",
                //        L("SchoolManagement"),
                //        url: "#",
                //        icon: "fal fa-info-circle",
                //        requiresAuthentication: true
                //    ).AddItem(
                //        new MenuItemDefinition(
                //            AppPageNames.AdminCommon.OrganizationUnits,
                //            L("Teachers"),
                //            url: "Teachers",
                //            icon: "fas fa-users",
                //            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Users)
                //        )
                //    ).AddItem(
                //        new MenuItemDefinition(
                //            AppPageNames.AdminCommon.Roles,
                //            L("Students"),
                //            url: "Students",
                //            icon: "fas fa-theater-masks",
                //            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Roles)
                //        )
                //    ).AddItem(
                //        new MenuItemDefinition(
                //            AppPageNames.AdminCommon.Users,
                //            L("Classes"),
                //            url: "Classes",
                //            icon: "fas fa-users",
                //            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Users)
                //        )
                //    ).AddItem(
                //        new MenuItemDefinition(
                //            AppPageNames.AdminCommon.Users,
                //            L("Settings"),
                //            url: "SchoolMgrSettings",
                //            icon: "fas fa-users",
                //            permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Administration_Users)
                //        )
                //    )
                //)

                // features Patient
                // features Crime with Drug
                // features CRM
                // features CMS
                // features SHOP & PRODUCTS
                // features Logistics
                ;
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, bbk.netcore.netcoreConsts.LocalizationSourceName);
        }
    }
}
