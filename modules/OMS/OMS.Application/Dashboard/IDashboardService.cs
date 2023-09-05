using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Contracts.Dto;
using bbk.netcore.mdl.OMS.Application.Dashboard.Dto;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Transfers.Dto;
using bbk.netcore.mdl.OMS.Application.UserWorkCounts.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Dashboard
{
    public interface IDashboardService : IApplicationService
    {
        Task<PagedResultDto<UserWorkCountCreateDto>> GetAllInDashBoard(DashboardInput input);

        Task<PagedResultDto<ContractListDto>> GetAllManager(DashboardInput input);

        Task<PagedResultDto<ImportRequestListDto>> GetAllThuKhoPn(DashboardInput input);

        Task<PagedResultDto<ExportRequestsListDto>> GetAllThuKhoPx(DashboardInput input);


        Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllTPKHTH(DashboardInput input);
        Task<PagedResultDto<ImportRequestSubListDto>> GetAllTPKHPN(DashboardInput input);
        Task<PagedResultDto<ExportRequestsListDto>> GetAllTPKHPx(DashboardInput input);

        Task<PagedResultDto<TransferListDto>> GetAllTranfer(DashboardInput input);

    }
}
