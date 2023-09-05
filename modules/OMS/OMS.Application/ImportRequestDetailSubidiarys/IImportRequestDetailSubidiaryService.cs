using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetailSubidiarys.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestDetailSubidiarys
{
    public interface IImportRequestDetailSubidiaryService : IApplicationService
    {
        Task<PagedResultDto<ImportRequestDetailSubListDto>> GetAll(GetInput input);
        Task<long> Create(ImportRequestDetailSubCreateDto input);
        Task<long> Update(ImportRequestDetailSubListDto input);
        Task<ImportRequestDetailSubListDto> GetAsync(EntityDto itemId);
        Task<PagedResultDto<ImportRequestDetailSubListDto>> GetAllItem();
    }
}
