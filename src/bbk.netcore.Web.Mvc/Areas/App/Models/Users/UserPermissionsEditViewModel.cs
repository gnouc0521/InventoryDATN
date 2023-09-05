using Abp.AutoMapper;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.Web.Areas.App.Models.Common;

namespace bbk.netcore.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; private set; }

        public UserPermissionsEditViewModel(GetUserPermissionsForEditOutput output, User user)
        {
            User = user;
            output.MapTo(this);
        }
    }
}