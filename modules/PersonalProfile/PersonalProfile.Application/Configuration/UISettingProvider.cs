using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Configuration
{
    class UISettingProvider: SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                {
                    new SettingDefinition(UIConsts.Header,"HỆ THỐNG QUẢN LÝ KHO BBK",scopes: SettingScopes.All, isVisibleToClients: true),
                    new SettingDefinition(UIConsts.Footer,"CÔNG TY CỔ PHẦN BBK",scopes: SettingScopes.All, isVisibleToClients: true),
                    new SettingDefinition(UIConsts.LogoUrl,"/img/logo.png",scopes: SettingScopes.All, isVisibleToClients: true),
                    new SettingDefinition(UIConsts.BannerUrl,"/img/white-img.jpg",scopes: SettingScopes.All, isVisibleToClients: true),
                    new SettingDefinition(UIConsts.LogoPath,"",scopes: SettingScopes.All, isVisibleToClients: false),
                    new SettingDefinition(UIConsts.BannerPath,"",scopes: SettingScopes.All, isVisibleToClients: false),
                };
        }
    }
}
