using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ExportRequests
{
    public interface IExportRequests : IApplicationService
    {
        Task<long> Create(ExportRequestsCreate input);
        Task<PagedResultDto<ExportRequestsListDto>> GetAll(ExportRequestsSearch input);
        Task<PagedResultDto<ExportRequestsListDto>> GetAllExport(ExportRequestsSearch input);
        Task<PagedResultDto<ExportRequestsListDto>> GetAllRequirement(ExportRequestsSearch input);
        Task<PagedResultDto<ExportRequestsListDto>> GetAllRequirementApprove(ExportRequestsSearch input);
        Task<ExportRequestsListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(ExportRequestsListDto input);
        Task<long> UpdateStatus(ExportRequestsListDto input);
        Task<long> UpdateExportStatus(ExportRequestsListDto input);
        Task<long> Delete(long id);

        // Code Ha them
        Task<ExportRequestsListDto> CreateToTransfer(ExportRequestsCreate input);
        Task<int> UpdateCodeSame(int idTransfer);
        Task<PagedResultDto<ExportRequestsListDto>> GetAllbyItemsId(ExportRequestsSearch input);



  }
}
