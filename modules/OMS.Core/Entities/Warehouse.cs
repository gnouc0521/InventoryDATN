using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppWareHouses", Schema = netcoreConsts.SchemaName)]
    public class Warehouse : FullAuditedEntity, ISoftDelete
    {
        [StringLength(500)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(10)]
        public string CityId { get; set; }

        [StringLength(10)]
        public string DistrictId { get; set; }

        [StringLength(10)]
        public string WardsId { get; set; }

        [StringLength(500)]
        public string Number { get; set; }

        [StringLength(10)]
        public string Lattitude { get; set; }

        [StringLength(10)]
        public string Longitude { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string TypeCode { get; set; }
        public bool DeleteFlag { get; set; }
        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }
        public WarehouseEnum.TypeWarehouse TypeWarehouse { get; set; }
        public ICollection<Items> items { get; set; }

    }
}
