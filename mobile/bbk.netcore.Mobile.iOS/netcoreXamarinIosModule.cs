using Abp.Modules;
using Abp.Reflection.Extensions;

namespace bbk.netcore
{
    [DependsOn(typeof(netcoreXamarinSharedModule))]
    public class netcoreXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(netcoreXamarinIosModule).GetAssembly());
        }
    }
}

