using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles
{
    public interface IPersonalProfileAppService : IApplicationService
    {
        Task<PersonalProfileDto> Get(int id);
        Task<List<PersonalProfileDto>> GetAllList();
        Task<Address> GetAddress(string filePath, string superiorId);
        Task UpdateImage(string imageURL,string imagePath,int id);
    }
}
