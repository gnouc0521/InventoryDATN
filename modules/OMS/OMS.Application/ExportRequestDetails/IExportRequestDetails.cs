using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ExportRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ExportRequestDetails
{
    public interface IExportRequestDetails : IApplicationService
    {
        Task<long> Create(ExportRequestDetailCreate input);
        Task<PagedResultDto<ExportRequestDetailListDto>> GetAll(ExportRequestDetailsSearch input);
        Task<PagedResultDto<ExportRequestDetailListDto>> GetAllExport(ExportRequestDetailsSearch input);
        Task<ExportRequestDetailListDto> GetAsync(EntityDto<long> itemId);
        Task<ExportRequestDetailListDto> GetDetail(EntityDto<long> itemId);
        Task<long> Update(ExportRequestDetailListDto input);
        Task<long> Delete(long id);
    }
}
