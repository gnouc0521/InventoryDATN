using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppInventory", Schema = netcoreConsts.SchemaName)]
    public class Inventory : FullAuditedEntity<long>     
    {
        public long  WarehouseId { get; set; }
        public long ItemId { get; set; }
        [StringLength(50)]
        public string ItemCode { get; set; }
        public long Quantity { get; set; }

    }
}
