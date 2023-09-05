using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppQuotes", Schema = netcoreConsts.SchemaName)]
    public class Quote : FullAuditedEntity<long>
    {
        public  long ItemId  { get; set; }
        [StringLength(250)]
        public string SupplierName { get; set; } 
        [StringLength(250)]
        public string Note { get; set; }
        public int UnitId { get; set; }
        public decimal QuotePrice { get; set; }
        public string Specifications { get; set; }
        public string SymbolCode { get; set; }
        public string UnitName { get; set; }
        public long Quantity { get; set; }
        public int SupplierId { get; set; }

    }
}
