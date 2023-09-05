using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.App.Models.Common
{
    public class OrganizationUnitTreeItemModel
    {
        public IOrganizationUnitsEditViewModel EditModel { get; set; }

        public long? ParentId { get; set; }

        public OrganizationUnitTreeItemModel()
        {

        }

        public OrganizationUnitTreeItemModel(IOrganizationUnitsEditViewModel editModel, long? parentId)
        {
            EditModel = editModel;
            ParentId = parentId;
        }
    }
}
