using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using bbk.netcore.Configuration;

namespace bbk.netcore.Web.Host.Startup
{
    [DependsOn(
       typeof(netcoreWebCoreModule))]
    public class netcoreWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public netcoreWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(netcoreWebHostModule).GetAssembly());
        }
    }
}
