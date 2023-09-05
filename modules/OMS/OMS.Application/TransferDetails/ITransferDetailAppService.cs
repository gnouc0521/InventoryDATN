using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.TransferDetails.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.TransferDetails
{
    public interface ITransferDetailAppService : IApplicationService
    {
        Task<long> Create(TransferDetailCreateDto input);
        Task<PagedResultDto<TransferDetailListDto>> GetAll(TransferDetailsSearch input);
        Task<PagedResultDto<TransferDetailListDto>> GetAllItem();
        Task<TransferDetailListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Delete(long Id);
        Task<long> Update(TransferDetailListDto input);

    }
}
