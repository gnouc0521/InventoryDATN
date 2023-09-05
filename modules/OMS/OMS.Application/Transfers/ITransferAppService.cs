using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.DayOffs.Dto;
using bbk.netcore.mdl.OMS.Application.Transfers.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Transfers
{
    public interface ITransferAppService : IApplicationService
    {
        Task<long> Create(TransferCreateDto input);
        Task<PagedResultDto<TransferListDto>> GetAll(TransferSearch input);
        Task<int> Delete(int id);

        Task<PagedResultDto<TransferListDto>> GetTransferImp();
        Task<PagedResultDto<TransferListDto>> GetTransferExp(TransferSearch input);
        Task<TransferListDto> GetAsync(EntityDto<long> itemId);
        Task<long> Update(TransferListDto input);
        Task<long> UpdateTransfer(TransferListDto input);

        Task<long> UpdateExportStatus(TransferListDto input);
        Task<long> UpdateTransferStatus(TransferListDto input);

        Task<PagedResultDto<TransferListDto>> GetAllApprove(TransferSearch input);

    }
}
