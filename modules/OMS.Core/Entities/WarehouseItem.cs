using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppWarehouseItems", Schema = netcoreConsts.SchemaName)]
    public class WarehouseItem : FullAuditedEntity
    {
        [StringLength(500)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }
     
        public long WarehouseId { get; set; }

        public long ParrentId { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string TypeCode { get; set; }

        [StringLength(10)]
        public string PositionX { get; set; }

        [StringLength(10)]
        public string PositionY { get; set; }

        [StringLength(10)]
        public string PositionZ { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        public decimal Lenght { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public decimal MaxKgVolume { get; set; }
        public decimal MaxM3Volume { get; set; }

        public bool DeleteFlag { get; set; }
        public int UnitId { get; set; }
        public int UnitMax { get; set; }
        public string CategoryCode { get; set; }
        public int WarehouseLevel { get; set; }
    }
}
