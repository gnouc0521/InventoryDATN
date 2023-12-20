using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WarehouseCards.Dto
{
  [AutoMap(typeof(bbk.netcore.mdl.OMS.Core.Entities.WarehouseCard))]
  public class WarehouseCardListDto : FullAuditedEntityDto<long>
  {
    public string Code { get; set; }
    public int WarehouseDestinationId { get; set; }
    public string  WarehouseDestinationName { get; set; }
    public int UnitId { get; set; }
    public string UnitName { get; set; }

    public long ItemsId { get; set; }

    public string ItemsName { get; set; }
    public long ItemsCode { get; set; }

    public WarehouseCardDetail WarehouseCardDetail { get; set; }
  }
}
