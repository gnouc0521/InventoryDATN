using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.OMS.Core;

namespace bbk.netcore.mdl.OMS.Application
{

    [DependsOn(
       typeof(netcoreCoreModule),
       typeof(OMSCoreModule),
       typeof(AbpAutoMapperModule))]
    public class OMSApplicationModule : AbpModule
    {
        
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<InventoryAuthorizationProvider>();
            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(OMSDtoMapper.CreateMappings);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(OMSApplicationModule).GetAssembly());
        }
    }
}
