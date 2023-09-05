using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using bbk.netcore.mdl.OMS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppInventoryTicket", Schema = netcoreConsts.SchemaName)]
    public class InventoryTicket: FullAuditedEntity, ISoftDelete
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime CompleteTime { get; set; }

        public long WarehouseId { get; set; }
    }
}
