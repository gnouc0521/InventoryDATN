using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.Contracts.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.Transfers.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Contracts
{
    public interface IContractAppService : IApplicationService
    {
        Task<PagedResultDto<ContractListDto>> GetAll(ContractSearch input);
        Task<PagedResultDto<ContractListDto>> GetAllInApprove(ContractSearch input);
        Task<int> Create(ContractCreateDto input);
        Task<ContractListDto> GetAsync(EntityDto itemId);
        Task<int> UpdateCodeSame(int idQuo);
        Task<long> UpdateContractStatus(ContractListDto input);
        Task<long> UpdateExportStatus(ContractListDto input);
        Task<long> UpdateContract(ContractListDto input);
        Task<PagedResultDto<QuoteListDto>> AllItemInContact(ContractSearch input);
        Task<decimal> TotalNumber(ContractSearch input);
        Task<string> CreateCode(int Id);

        Task<FileDto> GetPOListDto(int idContract);

    }
}
