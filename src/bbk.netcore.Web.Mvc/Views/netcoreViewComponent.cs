using Abp.AspNetCore.Mvc.ViewComponents;

namespace bbk.netcore.Web.Views
{
    public abstract class netcoreViewComponent : AbpViewComponent
    {
        protected netcoreViewComponent()
        {
            LocalizationSourceName = netcoreConsts.LocalizationSourceName;
        }
    }
}
