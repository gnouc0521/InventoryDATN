using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Statisticals.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<ReportDto> GetReport(StatisticalDto statisticalDto)
    {
      try
      {
        ReportDto returnObject = new ReportDto();


        var toDate = statisticalDto.DateTimeTo.Value;
        var fromDate = statisticalDto.DateTimeFrom.Value;

        for (int i = 0; i <= (toDate - fromDate).Days; i++)
        {
          returnObject.ListReportTaskDate.Insert(0, toDate.AddDays(-i).ToShortDateString());
          returnObject.ListReportQuantity.Add(0);
        }

        var query = _statisticalRepo
                    .GetAll()
                    .Where(x => x.DateStatistical <= toDate)
                    .Where(x => x.DateStatistical >= fromDate)
                    .WhereIf(statisticalDto.ItemsId.HasValue, u => u.ItemsId == statisticalDto.ItemsId.Value)
                    .WhereIf(statisticalDto.WarehouseId.HasValue, u => u.WarehouseId == statisticalDto.WarehouseId.Value)
                    .WhereIf(statisticalDto.DateStatistical.HasValue, u => u.DateStatistical == statisticalDto.DateStatistical.Value).ToList();
        var result = query.ToList();
        // tất cả workflow tạo ra
        for (int i = 0; i < result.Count(); i++)
        {
          for (int j = 0; j <= (toDate - fromDate).Days; j++)
          {
            if (result[i].DateStatistical.Date == fromDate.AddDays(+j).Date)
            {
              returnObject.ListReportQuantity[j] = result[j].Quantity;
            }
          }
        }

        return returnObject;
      }
      catch (Exception ex)
      {

        throw new UserFriendlyException(ex.Message); ;
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
