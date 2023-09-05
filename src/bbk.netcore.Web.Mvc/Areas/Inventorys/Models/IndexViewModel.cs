using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using bbk.netcore.Organizations.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Models
{
    public class IndexViewModel
    {
        public IReadOnlyList<OrganizationUnitDto> OrganizationUnits { get; set; }
    }
}
