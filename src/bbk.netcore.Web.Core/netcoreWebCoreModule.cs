using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using bbk.netcore.Authentication.JwtBearer;
using bbk.netcore.Configuration;
using bbk.netcore.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using bbk.netcore.mdl.PersonalProfile.Application;
using bbk.netcore.mdl.OMS.Application;
using bbk.netcore.mdl.OMS.Core;

namespace bbk.netcore
{
    [DependsOn(
        typeof(netcoreApplicationModule),
        typeof(netcoreEntityFrameworkModule),
        typeof(AbpAspNetCoreModule),
        typeof(AbpAspNetCoreSignalRModule),
        typeof(PersonalProfileApplicationModule),
        typeof(OMSApplicationModule)
     )]
    public class netcoreWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public netcoreWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                netcoreConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(netcoreApplicationModule).GetAssembly()
                 );
            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(PersonalProfileApplicationModule).GetAssembly()
                 
                 );
            Configuration.Modules.AbpAspNetCore()
               .CreateControllersForAppServices(
                   typeof(OMSApplicationModule).GetAssembly()

               );

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(netcoreWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(netcoreWebCoreModule).Assembly);
        }
    }
}
