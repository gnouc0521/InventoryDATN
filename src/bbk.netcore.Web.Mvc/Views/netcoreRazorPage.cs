using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace bbk.netcore.Web.Views
{
    public abstract class netcoreRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected netcoreRazorPage()
        {
            LocalizationSourceName = netcoreConsts.LocalizationSourceName;
        }
    }
}
