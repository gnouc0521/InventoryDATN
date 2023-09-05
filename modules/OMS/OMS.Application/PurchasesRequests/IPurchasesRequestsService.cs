using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchasesRequests
{
	public interface IPurchasesRequestsService : IApplicationService
	{
        Task<PagedResultDto<PurchasesRequestListDto>> GetAll(PurchasesRequestSearch input);
		Task<long> Update(PurchasesRequestListDto input);
		Task<PurchasesRequestListDto> GetAsync(EntityDto itemId);
		Task<int> Delete(int id);
	}
}
