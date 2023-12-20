using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
namespace bbk.netcore.mdl.OMS.Application.WarehouseCards.Dto
{
  [AutoMap(typeof(WarehouseCard))]
  public class WarehouseCardCreate : FullAuditedEntityDto<long>
  {
    public string Code { get; set; }
    public int WarehouseDestinationId { get; set; }
    public int UnitId { get; set; }

    public long ItemsId { get; set; }
  }
}
