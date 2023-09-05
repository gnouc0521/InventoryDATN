using Abp.AspNetCore;
using Abp.Modules;
using Abp.Resources.Embedded;
using bbk.netcore.mdl.PersonalProfile.Application;
using bbk.netcore.mdl.PersonalProfile.Web.Navigation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Web
{
    [DependsOn(
        typeof(PersonalProfileApplicationModule),
        //typeof(PersonalProfileEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreModule))]
    public class PersonalProfileWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<PersonalProfileNavigationProvider>();

            Configuration.EmbeddedResources.Sources.Add(
                new EmbeddedResourceSet(
                    "/Areas/App/Views/",
                    Assembly.GetExecutingAssembly(),
                    "bbk.netcore.mdl.PersonalProfile.Web.Areas.App.Views"
                )
            );

            //Must call app.UseEmbeddedFiles() at main application Configure!
            Configuration.EmbeddedResources.Sources.Add(
                new EmbeddedResourceSet(
                    "/Resources/",
                    Assembly.GetExecutingAssembly(),
                    "bbk.netcore.mdl.PersonalProfile.Web.Resources"
                    )
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
