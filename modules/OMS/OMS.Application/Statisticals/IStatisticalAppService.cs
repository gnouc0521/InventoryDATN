using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Statisticals.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Statisticals
{
  public interface IStatisticalAppService : IApplicationService
  {
    public Task Create (StatisticalDto statisticalDto);
    public Task<PagedResultDto<StatisticalDto>> GetTask (StatisticalDto statisticalDto);
  }
}
