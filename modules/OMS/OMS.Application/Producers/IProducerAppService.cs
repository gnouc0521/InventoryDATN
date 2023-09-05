using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Producers
{
    public interface IProducerAppService : IApplicationService
    {
        Task<long> Create(ProducerCreateDto input);
        Task<PagedResultDto<ProducerListDto>> GetAll(GetProducerInput input);
        Task<long> Update(ProducerListDto input);
        Task<int> Delete(int id);

        Task<ProducerListDto> GetAsync(EntityDto itemId);
        Task<List<ProducerListDto>> GetProducerList();

        Task<Address> GetAddress(string filePath, string superiorId);
    }
}
