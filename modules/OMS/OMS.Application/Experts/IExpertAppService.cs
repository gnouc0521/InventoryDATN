using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.mdl.OMS.Application.Experts.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Experts
{
    public interface IExpertAppService : IApplicationService
    {
        Task<int> Create(ExpertCreateDto input);
        Task<PagedResultDto<ExpertListDto>> GetAll(GetExpertInput input);
        Task<ExpertListDto> GetAsync(EntityDto Id);
        Task<int> Update(ExpertEditDto input);
        Task<PagedResultDto<UserListDto>> GetAllUser();
        Task<PagedResultDto<ItemsListDto>> GetAllItem();
        Task<PagedResultDto<ItemsListDto>> GetAllItemExpert(int Id);
        Task<PagedResultDto<ExpertListDto>> GetAllUserALL();
    }
}
