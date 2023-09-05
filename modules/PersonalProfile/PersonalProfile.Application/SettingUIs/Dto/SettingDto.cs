using Abp.AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Settings.Dto
{
    public class SettingDto
    {
        public string LogoUrl { get; set; }
        public string BannerUrl { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }

    }
}
