using System;
using bbk.netcore.Core;
using bbk.netcore.Core.Dependency;
using bbk.netcore.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bbk.netcore.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}

