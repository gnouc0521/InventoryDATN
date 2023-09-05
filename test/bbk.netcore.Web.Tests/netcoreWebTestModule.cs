using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using bbk.netcore.EntityFrameworkCore;
using bbk.netcore.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace bbk.netcore.Web.Tests
{
    [DependsOn(
        typeof(netcoreWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class netcoreWebTestModule : AbpModule
    {
        public netcoreWebTestModule(netcoreEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(netcoreWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(netcoreWebMvcModule).Assembly);
        }
    }
}