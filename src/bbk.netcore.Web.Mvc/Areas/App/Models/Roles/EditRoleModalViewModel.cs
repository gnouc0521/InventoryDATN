using Abp.AutoMapper;
using bbk.netcore.Authorization.Permissions.Dto;
using bbk.netcore.Authorization.Roles.Dto;
using bbk.netcore.Web.Areas.App.Models.Common;

namespace bbk.netcore.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool HasPermission(FlatPermissionDto permission)
        {
            return GrantedPermissionNames.Contains(permission.Name);
        }
    }
}
