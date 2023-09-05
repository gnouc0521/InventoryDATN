using Abp.Application.Services;
using bbk.netcore.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports
{
    public interface IReportAppService : IApplicationService
    {
        Task<FileDto> Report(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM01aListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM02ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM03ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM04ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM07ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM08ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM09ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM10ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM12ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> GetBM15ListDto(ReportFilterDto reportFilterDto);
        Task<FileDto> ExportHS02(ExportHS02Dto person);
    }
}
