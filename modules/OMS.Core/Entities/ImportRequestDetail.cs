using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppImportRequestDetail", Schema = netcoreConsts.SchemaName)]
    public class ImportRequestDetail : FullAuditedEntity
    {
        public int ImportRequestId { get; set; }
        public virtual ImportRequest ImportRequest { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public int QuantityHT { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal ImportPrice { get; set; }
        public DateTime ExpireDate { get; set; }

      

        // public DateTime MFG { get; set; }

        // public long ParcelId { get; set; }

        //  [StringLength(500)]
        //public string ParcelCode { get; set; }

        //[StringLength(1000)]
        //public string Remark { get; set; }
    }
}
