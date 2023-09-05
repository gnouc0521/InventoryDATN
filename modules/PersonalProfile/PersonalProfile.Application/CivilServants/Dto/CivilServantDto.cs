using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.CivilServants.Dto
{
    [AutoMapFrom(typeof(CivilServant))]
    public class CivilServantDto : EntityDto
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public CivilServantGroup Group { get; set; }
    }

    public class GetAllCivilServantInput
    {
        public string Keyword { get; set; }
    }
}
