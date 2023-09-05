using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM10Dto
    {
        public int STT { get; set; }
        public string FullName { get; set; }
        public string CommendationTitle1 { get; set; }
        public string CommendationTitle2 { get; set; }
        public string CommendationTitle3 { get; set; }
    }
}
