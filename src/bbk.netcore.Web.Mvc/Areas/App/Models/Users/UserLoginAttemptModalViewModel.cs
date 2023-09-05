using System.Collections.Generic;
using bbk.netcore.Authorization.Users.Dto;

namespace bbk.netcore.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}