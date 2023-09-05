using System.Collections.Generic;
using bbk.netcore.Authorization.Permissions.Dto;
using bbk.netcore.Authorization.Roles.Dto;

namespace bbk.netcore.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
        List<string> GrantedPermissionNames { get; set; }
    }
}