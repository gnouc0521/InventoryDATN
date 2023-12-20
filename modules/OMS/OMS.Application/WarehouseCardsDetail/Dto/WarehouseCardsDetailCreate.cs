using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WarehouseCardsDetail.Dto
{
    [AutoMap(typeof(WarehouseCardDetail))]
    public class WarehouseCardsDetailCreate : FullAuditedEntityDto<long>
  {
    public long WarehouseId { get; set; }
    public long ImportRequestId { get; set; }
    public long ExportRequestId { get; set; }
    public long ExportDate { get; set; }
    public long ImportDate { get; set; }
    public string Remark { get; set; }
    public long ImportQuantity { get; set; }
    public long ExportQuantity { get; set; }

    public long Inventory { get; set; }
  }
}
