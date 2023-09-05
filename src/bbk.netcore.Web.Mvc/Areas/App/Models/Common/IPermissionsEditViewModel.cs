using bbk.netcore.Authorization.Permissions.Dto;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}