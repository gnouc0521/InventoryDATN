using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.StaffPlainnings.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.StaffPlainnings
{
    public interface IStaffPlainningAppService : IApplicationService
    {
        Task<int> Create(StaffPlainningDto staffPlainningDto);

        Task Update(StaffPlainningDto staffPlainningDto);

        Task<List<GetStaffPlainningDto>> GetAll(GetAllFilter filter);

        Task<StaffPlainningDto> GetById(int id);

        Task DeleteById(DeleteStaffPlainningDto deleteStaffPlainningDto);
    }
}
