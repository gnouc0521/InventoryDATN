using Abp.Modules;
using Abp.Reflection.Extensions;

namespace bbk.netcore.mdl.PersonalProfile.Core
{
    public class PersonalProfileCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PersonalProfileCoreModule).GetAssembly());
        }
    }
}
