using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppExportRequestDetail", Schema = netcoreConsts.SchemaName)]
    public class ExportRequestDetail : FullAuditedEntity<long>
    {
        public long ExportRequestId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal ExportPrice { get; set; }
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
