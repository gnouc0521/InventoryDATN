using System.Collections.Generic;
using System.Linq;
using Abp.AutoMapper;
using AutoMapper;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.Security;
using bbk.netcore.Web.Areas.App.Models.Common;

namespace bbk.netcore.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        private readonly IMapper _mapper;
        public bool CanChangeUserName
        {
            get { return User.UserName != Authorization.Users.User.AdminUserName; }
        }

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }

        public bool IsEditMode
        {
            get { return User.Id.HasValue; }
        }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public CreateOrEditUserModalViewModel(GetUserForEditOutput output)
        {
            //ObjectMapper<output>
            output.MapTo(this);
        }
    }
}