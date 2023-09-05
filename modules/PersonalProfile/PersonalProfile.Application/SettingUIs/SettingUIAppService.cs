using Abp.Application.Services;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.UI;
using bbk.netcore.mdl.PersonalProfile.Application.Configuration;
using bbk.netcore.mdl.PersonalProfile.Application.Settings.Dto;
using bbk.netcore.Storage.FileSystem;
using System;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.Settings
{
    [AbpAuthorize]
    public class SettingUIAppService : ApplicationService, ISettingUIAppService

    {
        private readonly ISettingManager _settingManager;
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        public SettingUIAppService(ISettingManager settingManager, IFileSystemBlobProvider fileSystemBlobProvider, ISettingDefinitionManager settingDefinitionManager)
        {
            _settingManager = settingManager;
            _fileSystemBlobProvider = fileSystemBlobProvider;
            _settingDefinitionManager = settingDefinitionManager;
        }

        public SettingDto Get()
        {
            var header = _settingManager.GetSettingValue(UIConsts.Header);
            var footer = _settingManager.GetSettingValue(UIConsts.Footer);
            var logoUrl = _settingManager.GetSettingValue(UIConsts.LogoUrl);
            var bannerUrl = _settingManager.GetSettingValue(UIConsts.BannerUrl);
            return new SettingDto
            {
                Header = header,
                Footer = footer,
                LogoUrl = logoUrl,
                BannerUrl = bannerUrl
            };
        }

        public async Task Reset()
        {
            try
            {
                var header = _settingDefinitionManager.GetSettingDefinition(UIConsts.Header);
                var footer = _settingDefinitionManager.GetSettingDefinition(UIConsts.Footer);
                var logoUrl = _settingDefinitionManager.GetSettingDefinition(UIConsts.LogoUrl);
                var logoPath = _settingDefinitionManager.GetSettingDefinition(UIConsts.LogoPath);
                var bannerUrl = _settingDefinitionManager.GetSettingDefinition(UIConsts.BannerUrl);
                var bannerPath = _settingDefinitionManager.GetSettingDefinition(UIConsts.BannerPath);

                var oldLogoPath = _settingManager.GetSettingValue(UIConsts.LogoPath);
                var oldBannerPath = _settingManager.GetSettingValue(UIConsts.BannerPath);
                if (!string.IsNullOrWhiteSpace(oldLogoPath))
                {
                    await _fileSystemBlobProvider.DeleteAsync(oldLogoPath);
                }
                if (!string.IsNullOrWhiteSpace(oldBannerPath))
                {
                    await _fileSystemBlobProvider.DeleteAsync(oldBannerPath);
                }
                await _settingManager.ChangeSettingForApplicationAsync(UIConsts.Header, header.DefaultValue);
                await _settingManager.ChangeSettingForApplicationAsync(UIConsts.Footer, footer.DefaultValue);
                await _settingManager.ChangeSettingForApplicationAsync(UIConsts.LogoUrl, logoUrl.DefaultValue);
                await _settingManager.ChangeSettingForApplicationAsync(UIConsts.LogoPath, logoPath.DefaultValue);
                await _settingManager.ChangeSettingForApplicationAsync(UIConsts.BannerUrl, bannerUrl.DefaultValue);
                await _settingManager.ChangeSettingForApplicationAsync(UIConsts.BannerPath, bannerPath.DefaultValue);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [UnitOfWork]
        public async Task Update(UpdateDto dto)
        {
            try
            {
                await _settingManager.ChangeSettingForApplicationAsync(UIConsts.Header, dto.Header);
                await _settingManager.ChangeSettingForApplicationAsync(UIConsts.Footer, dto.Footer);
                if(!string.IsNullOrWhiteSpace(dto.BannerUrl) && !string.IsNullOrWhiteSpace(dto.BannerPath))
                {
                    if (!string.IsNullOrWhiteSpace(_settingManager.GetSettingValue(UIConsts.BannerPath)))
                    {
                        await _fileSystemBlobProvider.DeleteAsync(_settingManager.GetSettingValue(UIConsts.BannerPath));
                    }
                    await _settingManager.ChangeSettingForApplicationAsync(UIConsts.BannerUrl, dto.BannerUrl);
                    await _settingManager.ChangeSettingForApplicationAsync(UIConsts.BannerPath, dto.BannerPath);
                }
                if (!string.IsNullOrWhiteSpace(dto.LogoUrl) && !string.IsNullOrWhiteSpace(dto.LogoPath))
                {
                    if (!string.IsNullOrWhiteSpace(_settingManager.GetSettingValue(UIConsts.LogoPath)))
                    {
                        await _fileSystemBlobProvider.DeleteAsync(_settingManager.GetSettingValue(UIConsts.LogoPath));
                    }
                    await _settingManager.ChangeSettingForApplicationAsync(UIConsts.LogoUrl, dto.LogoUrl);
                    await _settingManager.ChangeSettingForApplicationAsync(UIConsts.LogoPath, dto.LogoPath);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
