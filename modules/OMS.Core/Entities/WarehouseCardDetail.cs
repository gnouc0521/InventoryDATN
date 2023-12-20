using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.OMS.Core.Enums.ExportEnums;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
  [Table("WarehouseCardDetail", Schema = netcoreConsts.SchemaName)]
  public class WarehouseCardDetail : FullAuditedEntity<long>
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
