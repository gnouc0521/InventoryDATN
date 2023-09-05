using Abp.Application.Services;
using bbk.netcore.mdl.OMS.Application.UserWorkCounts.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.UserWorkCounts
{
    public interface IUserWorkCountService : IApplicationService
    {
        Task<int> Create(UserWorkCountCreateDto input);
        Task<long> UpdateRequest(UserWorkCountListDto input);
        Task<long> UpdateUserId(UserWorkCountListDto input);
        Task<long> UpdateSys(UserWorkCountListDto input);
    }
}
