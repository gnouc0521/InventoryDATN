using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Statisticals.Dto
{
    [AutoMap(typeof(Statistical))]

  public class StatisticalDto : EntityDto
  {
    public long? ItemsId { get; set; }
    public long? WarehouseId { get; set; }
    public int Quantity { get; set; }
    public DateTime? DateStatistical { get; set; }
  }
}
