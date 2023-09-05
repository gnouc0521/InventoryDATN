using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using bbk.netcore.Configuration;
using bbk.netcore.Web.Areas.App.Startup;
using bbk.netcore.mdl.PersonalProfile.Application;
using bbk.netcore.mdl.PersonalProfile.Web.Navigation;
using bbk.netcore.EntityFrameworkCore;
using bbk.netcore.mdl.OMS.Application;

namespace bbk.netcore.Web.Startup
{
    [DependsOn(typeof(netcoreWebCoreModule), typeof(PersonalProfileApplicationModule),typeof(OMSApplicationModule))]
    public class netcoreWebMvcModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public netcoreWebMvcModule(IWebHostEnvironment env, netcoreEntityFrameworkModule abpZeroTemplateEntityFrameworkCoreModule)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
            abpZeroTemplateEntityFrameworkCoreModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            // Đăng ký navigation
            Configuration.Navigation.Providers.Add<netcoreNavigationProvider>();
            Configuration.Navigation.Providers.Add<AppNavigationProvider>();
            #region Personal Profile
            Configuration.Navigation.Providers.Add<InventorysNavigationProvider>();
            #endregion
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(netcoreWebMvcModule).GetAssembly());
        }
    }
}
