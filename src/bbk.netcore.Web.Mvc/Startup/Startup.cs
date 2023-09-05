using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Castle.Logging.Log4Net;
using bbk.netcore.Authentication.JwtBearer;
using bbk.netcore.Configuration;
using bbk.netcore.Identity;
using bbk.netcore.Web.Resources;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Abp.Json;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace bbk.netcore.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IConfiguration Configuration;
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _appConfiguration = env.GetAppConfiguration();
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddControllersWithViews(
                    options =>
                    {
                        //options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                        //options.Filters.Add<AbpAutoValidateAntiforgeryTokenAttribute>();
                    }
                )
                .AddControllersAsServices()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                ;

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddScoped<IWebResourceManager, WebResourceManager>();
     
            services.AddSignalR();

            services.AddOptions();                                         // Kích hoạt Options
            var mailsettings = Configuration.GetSection("MailSettings");  // đọc config
            services.Configure<MailSettings>(mailsettings);                // đăng ký để Inject
            services.AddTransient<ISendMailService, SendMailService>();

            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Inventorys/Login");

            /*--------------------------------------------------------------------------------------------------------------------*/
            /*                      Anti Forgery Token Validation Service                                                         */
            /* We use the option patterm to configure the Antiforgery feature through the AntiForgeryOptions Class                */
            /* The HeaderName property is used to specify the name of the header through which antiforgery token will be accepted */
            /*--------------------------------------------------------------------------------------------------------------------*/
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<netcoreWebMvcModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IAntiforgery antiforgery)
        {
            app.UseAbp(); // Initializes ABP framework.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            // 3:11 PM 1/8/2021: https://github.com/techhowdy/CMS_CORE_NG/blob/master/CMS_CORE_NG/Startup.cs
            app.UseHttpsRedirection();
            app.UseCors("EnableCORS");

            app.UseStaticFiles();
            app.UseEmbeddedFiles(); //Allows to expose embedded files to the web!

            app.UseRouting();

            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseAuthorization();

            // 3:11 PM 1/8/2021: https://github.com/techhowdy/CMS_CORE_NG/blob/master/CMS_CORE_NG/Startup.cs
            /* Configure the app to provide a token in a cookie called XSRF-TOKEN */
            /* Custom Middleware Component is required to Set the cookie which is named XSRF-TOKEN 
             * The Value for this cookie is obtained from IAntiForgery service
             * We must configure this cookie with HttpOnly option set to false so that browser will allow JS to read this cookie
             */
            app.Use(nextDelegate =>
            {
                RequestDelegate requestDelegate = context =>
                               {
                                   string path = context.Request.Path.Value.ToLower();
                                   string[] directUrls = { "/app", "/Inventorys/Login", "/Inventorys", "checkout", "/account/login" };
                                   if (path.StartsWith("/swagger") || path.StartsWith("/api") || string.Equals("/", path) || directUrls.Any(url => path.StartsWith(url)))
                                   {
                                       AntiforgeryTokenSet tokens = antiforgery.GetAndStoreTokens(context);
                                       context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions()
                                       {
                                           HttpOnly = false,
                                           Secure = false,
                                           IsEssential = true,
                                           SameSite = SameSiteMode.Strict
                                       });

                                   }

                                   return nextDelegate(context);
                               };
                return requestDelegate;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                //endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                // 3:11 PM 1/8/2021: https://github.com/techhowdy/CMS_CORE_NG/blob/master/CMS_CORE_NG/Startup.cs
                endpoints.MapControllerRoute(
                   name: "areas",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
