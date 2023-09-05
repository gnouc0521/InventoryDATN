using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppWarehouseLocationItems", Schema = netcoreConsts.SchemaName)]
    public class WarehouseLocationItems : FullAuditedEntity<long>, ISoftDelete
    {
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }

        [StringLength(50)]
        public string ItemCode { get; set; }
        public long ParcelId { get; set; }

        [StringLength(50)]
        public string Block { get; set; }

        [StringLength(50)]
        public string Shelf { get; set; }

        [StringLength(50)]
        public string Floor { get; set; }

        [StringLength(250)]
        public string DescriptionLocation { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpireDate { get; set; }
        public long ImportRequestId { get; set; }
        public long ImportRequestDetailId { get; set; }
        public DateTime ImportDate { get; set; }
        public bool IsItems { get; set; }
        public int QuantityReality { get; set; }
        public string Note { get; set; }

    }
}
