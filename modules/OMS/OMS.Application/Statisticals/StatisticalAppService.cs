using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.Statisticals.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Statisticals
{
  public class StatisticalAppService : ApplicationService, IStatisticalAppService
  {
    private readonly IRepository<Statistical> _statisticalRepo;
    public StatisticalAppService(IRepository<Statistical> statisticalRepo)
    {
      _statisticalRepo = statisticalRepo;
    }

    public async Task Create(StatisticalDto statisticalDto)
    {
      try
      {
        Statistical statistical = ObjectMapper.Map<Statistical>(statisticalDto);
        _statisticalRepo.Insert(statistical);
      }
      catch (Exception ex)
      {
        throw new UserFriendlyException(ex.Message);
      }
    }

    public async Task<PagedResultDto<StatisticalDto>> GetTask(StatisticalDto statisticalDto)
    {
      try
      {
        var query = _statisticalRepo
                     .GetAll()

                     .WhereIf(statisticalDto.ItemsId.HasValue , u => u.ItemsId == statisticalDto.ItemsId.Value)
                     .WhereIf(statisticalDto.WarehouseId.HasValue , u => u.WarehouseId == statisticalDto.WarehouseId.Value)
                     .WhereIf(statisticalDto.DateStatistical.HasValue , u => u.DateStatistical == statisticalDto.DateStatistical.Value).ToList();
        var rulesCount = query.Count();
        var rulesListDto = ObjectMapper.Map<List<StatisticalDto>>(query);
        return new PagedResultDto<StatisticalDto>(
          rulesCount,
          rulesListDto
          );
      }
      catch (Exception ex)
      {

        throw new UserFriendlyException(ex.Message);
      }
    }
  }
}
