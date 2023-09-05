using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace bbk.netcore
{
    [DependsOn(typeof(netcoreClientModule), typeof(AbpAutoMapperModule))]
    public class netcoreXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(netcoreXamarinSharedModule).GetAssembly());
        }
    }
}

