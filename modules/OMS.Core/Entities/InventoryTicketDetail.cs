using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppInventoryTicketDetail", Schema = netcoreConsts.SchemaName)]
    public class InventoryTicketDetail : FullAuditedEntity, ISoftDelete
    {

        public int InventoryTicketId { get; set; }
        public virtual InventoryTicket InventoryTicket { get; set; }

        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public int UnitName { get; set; }
        public DateTime ExpireDate { get; set; }
        public long ParcelId { get; set; }

        [StringLength(500)]
        public string ParcelCode { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }
       
        public long WarehouseSourceId { get; set; }
        public long BlockId { get; set; }

        public long FloorId { get; set; }

        public long ShelfId { get; set; }

    }
}
