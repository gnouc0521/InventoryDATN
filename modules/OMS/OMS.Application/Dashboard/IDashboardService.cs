using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Dashboard.Dto;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Dashboard
{
  public interface IDashboardService : IApplicationService
    {
        Task<PagedResultDto<ImportRequestListDto>> GetAllThuKhoPn(DashboardInput input);

        Task<PagedResultDto<ExportRequestsListDto>> GetAllThuKhoPx(DashboardInput input);


        Task<PagedResultDto<ImportRequestSubListDto>> GetAllTPKHPN(DashboardInput input);
        Task<PagedResultDto<ExportRequestsListDto>> GetAllTPKHPx(DashboardInput input);

        //Task<PagedResultDto<TransferListDto>> GetAllTranfer(DashboardInput input);

    }
}
