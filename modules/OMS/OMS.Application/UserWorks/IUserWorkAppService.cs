using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Application.UserWorks.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WorkUsers
{
    public interface IUserWorkAppService : IApplicationService
    {
        Task<int> Create(UserWorkCreateDto input);

        Task<PagedResultDto<UserWorkListDto>> GetAll(GetUserWorkInput input);

        //Task<PagedResultDto<UserWorkListDto>> GetAllUserWork();
        Task<PagedResultDto<UserWorkListDto>> GetUserIdByWorkId(int workId);

        Task<int> Delete(int userId);
    }
}
