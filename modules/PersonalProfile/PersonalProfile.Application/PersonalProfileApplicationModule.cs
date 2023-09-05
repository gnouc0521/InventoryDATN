using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.Configuration;
using bbk.netcore.mdl.PersonalProfile.Core;
using System;

namespace bbk.netcore.mdl.PersonalProfile.Application
{
    [DependsOn(
        typeof(netcoreCoreModule),
        typeof(PersonalProfileCoreModule),
        typeof(AbpAutoMapperModule))]
    public class PersonalProfileApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            //Using default permission of netcore bkk
            //Configuration.Authorization.Providers.Add<PersonalProfileAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(PersonalProfileDtoMapper.CreateMappings);
            Configuration.Settings.Providers.Add<UISettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PersonalProfileApplicationModule).GetAssembly());
        }
    }
}
