using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;

namespace bbk.netcore.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly netcoreDbContext _context;

        public DefaultSettingsCreator(netcoreDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            if (netcoreConsts.MultiTenancyEnabled == false)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "tung.tran84@outlook.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "tung.tran", tenantId);

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
