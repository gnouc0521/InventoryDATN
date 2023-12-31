using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequests.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ImportRequests
{
    public interface IImportRequestAppService : IApplicationService
    {
        Task<PagedResultDto<ImportRequestListDto>> GetAll(GetImportRequestInput input);
        Task<long> Create(ImportRequestCreateDto input);
        Task<long> Update(ImportRequestListDto input);
        Task<int> Delete(int id);
        Task<ImportRequestListDto> GetAsync(EntityDto itemId);

        //code Ha
        Task<ImportRequestListDto> CreateToTranssfer(ImportRequestCreateDto input);
        Task<long> UpdateStatusImport(ImportRequestListDto input);
        Task<long> UpdateSunmit(ImportRequestListDto input);
       Task<PagedResultDto<ImportRequestListDto>> GetAllByItems(GetImportRequestInput input);




  }
}
