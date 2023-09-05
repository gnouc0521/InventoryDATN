using System.Collections.Generic;
using Abp.Application.Services.Dto;
using bbk.netcore.Authorization.Roles.Dto;

namespace bbk.netcore.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel
    {
        //public IReadOnlyList<PermissionDto> Permissions { get; set; }
        public List<ComboboxItemDto> Permissions { get; set; }
    }
}
