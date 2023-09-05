using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.App.Models.OrganizationUnits
{
    public class CreateOrganizationUnitModalViewModel
    {
        public long? ParentId { get; set; }

        public CreateOrganizationUnitModalViewModel(long? parentId)
        {
            ParentId = parentId;
        }
    }
}
