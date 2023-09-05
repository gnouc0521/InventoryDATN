using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.CivilServants.Dto
{
    [AutoMapFrom(typeof(SalaryLevel))]
    public class SalaryLevelDto : EntityDto
    {
        public string Level { get; set; }

        public string CoefficientsSalary { get; set; }

        public CivilServantGroup Group { get; set; }
    }
}
