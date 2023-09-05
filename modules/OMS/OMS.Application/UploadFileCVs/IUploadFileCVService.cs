using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Application.UploadFileCVs.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.UploadFileCVs
{
    public interface IUploadFileCVService :IApplicationService
    {
        Task<PagedResultDto<UploadFileListDto>> GetAllById(UploadFileListDto input);
    }
}
