using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace bbk.netcore.mdl.OMS.Core
{
    public class OMSCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(OMSCoreModule).GetAssembly());
        }
    }
}
