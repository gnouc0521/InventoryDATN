using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails
{
	public interface IPurchasesRequestDetailService : IApplicationService
	{
        Task<PagedResultDto<PurchasesRequestDetailListDto>> GetAll(PurchasesRequestDetailSearch input);
		Task<long> Create(PurchasesRequestDetailCreateDto input);
		Task<long> Update(PurchasesRequestDetailListDto input);
		Task<long> UpdateQuantity(PurchasesRequestDetailListDto input);
		Task<PurchasesRequestDetailListDto> GetAsync(EntityDto itemId);
		Task<int> Delete(int id);
	}
}
