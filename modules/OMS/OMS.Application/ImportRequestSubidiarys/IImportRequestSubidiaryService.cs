using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys
{
    public interface IImportRequestSubidiaryService : IApplicationService
    {
        Task<PagedResultDto<ImportRequestSubListDto>> GetAll(GetInput input);
        Task<long> Create(ImportRequestSubCreateDto input);
        Task<long> Update(ImportRequestSubListDto input);
        Task<int> Delete(int id);
        Task<ImportRequestSubListDto> GetAsync(EntityDto itemId);
        Task<long> UpdateStatus(ImportRequestSubListDto input);

        Task<PagedResultDto<ImportRequestSubListDto>> GetAllDone(GetInput input);

        Task<long> UpdateStatusIMP(ImportRequestSubListDto input);
    }
}
