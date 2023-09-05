using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.App.Startup
{
    public class AppPageNames
    {
        public static class AdminCommon
        {
            public const string Administration = "Administration";
            public const string Roles = "Administration.Roles";
            public const string Users = "Administration.Users";
            public const string AuditLogs = "Administration.AuditLogs";
            public const string OrganizationUnits = "Administration.OrganizationUnits";
            public const string Languages = "Administration.Languages";
            //public const string DemoUiComponents = "Administration.DemoUiComponents";
            //public const string UiCustomization = "Administration.UiCustomization";
            public const string Settings = "Administration.Settings";
        }

        public static class Host
        {
            public const string Tenants = "Tenants";
            public const string Editions = "Editions";
            public const string Maintenance = "Administration.Maintenance";
            public const string Settings = "Administration.Settings.Host";
            public const string Dashboard = "Dashboard";
        }

        public static class Tenant
        {
            public const string Dashboard = "Dashboard.Tenant";
            public const string Settings = "Administration.Settings.Tenant";
            public const string SubscriptionManagement = "Administration.SubscriptionManagement.Tenant";
        }

        public static class PersonalProfileModule
        {
            public const string PersonalProfile_Management = "PersonalProfile.Management";
        }

        public static class CMS
        {
            //public const string ArticleMgr = "Article.Management";
            public const string ArticleMgt = "ArticleMgt";
        }
    }
}
