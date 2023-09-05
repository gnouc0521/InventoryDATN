using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.Settings.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.Settings
{
    public interface ISettingUIAppService : IApplicationService
    {
        SettingDto Get();
        Task Update(UpdateDto dto);

        Task Reset();
    }
}
