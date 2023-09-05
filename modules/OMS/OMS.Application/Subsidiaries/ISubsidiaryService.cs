using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Subsidiaries
{
    public interface ISubsidiaryService : IApplicationService
    {
        Task<PagedResultDto<SubsidiaryListDto>> GetAll(SubsidiarySearch input);
        Task<long> Create(SubsidiaryCreateDto input);
        Task<long> Update(SubsidiaryListDto input);
        Task<SubsidiaryListDto> GetAsync(EntityDto itemId);
        Task<int> Delete(int id);

        Task<Address> GetAddress(string filePath, string superiorId);
        Task<PagedResultDto<SubsidiaryListDto>> GetAllSub();
        Task<List<SubsidiaryListDto>> GetSubsidiaryList();

        Task<PagedResultDto<SubsidiaryListDto>> GetAllPurchese(SubsidiarySearch input);
    }
}
