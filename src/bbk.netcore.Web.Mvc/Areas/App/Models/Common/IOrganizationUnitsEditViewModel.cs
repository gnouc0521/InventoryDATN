using bbk.netcore.Organizations.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.App.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }
    }
}
