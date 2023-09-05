using Abp.Modules;
using Abp.Reflection.Extensions;

namespace bbk.netcore
{
    [DependsOn(typeof(netcoreXamarinSharedModule))]
    public class netcoreXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(netcoreXamarinAndroidModule).GetAssembly());
        }
    }
}

