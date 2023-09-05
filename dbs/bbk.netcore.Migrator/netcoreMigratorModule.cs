using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using bbk.netcore.Configuration;
using bbk.netcore.EntityFrameworkCore;
using bbk.netcore.Migrator.DependencyInjection;
using System;

namespace bbk.netcore.Migrator
{
    [DependsOn(typeof(netcoreEntityFrameworkModule))]
    public class netcoreMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public netcoreMigratorModule(netcoreEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            bool configSkipDbSeed = false;
            abpProjectNameEntityFrameworkModule.SkipDbSeed = false;

            _appConfiguration = AppConfigurations.Get(
                typeof(netcoreMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );

            // configurable
            try
            {
                bool.TryParse(_appConfiguration.GetSection("MigrationSettings:SkipDbSeed").Value, out configSkipDbSeed);
                abpProjectNameEntityFrameworkModule.SkipDbSeed = configSkipDbSeed;
            }
            catch (Exception ex) { throw ex; }
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                netcoreConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus),
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(netcoreMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
