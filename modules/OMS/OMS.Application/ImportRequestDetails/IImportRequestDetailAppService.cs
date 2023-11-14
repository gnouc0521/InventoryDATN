using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestDetails
{
  public interface IImportRequestDetailAppService : IApplicationService
    {
        Task<PagedResultDto<ImportRequestDetailListDto>> GetAll(GetImportRequestDetailInput input);
        Task<long> Create(ImportRequestDetailCreateDto input);
        Task<long> Update(ImportRequestDetailListDto input);
        Task<int> Delete(int id);
        Task<ImportRequestDetailListDto> GetAsync(EntityDto itemId);

        //Task<PagedResultDto<ImportRequestDetailListDto>> GetAllItem(GetImportRequestDetailInput input);
    }
}
