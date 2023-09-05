using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppTransferDetails", Schema = netcoreConsts.SchemaName)]
    public class TransferDetail : FullAuditedEntity<long>
    {
        public long TransferId { get; set; }
       
        public int ItemId { get; set; }
        [StringLength(250)]
        public string ItemName { get; set; }
        [StringLength(50)]
        public string ItemCode { get; set; }
        public decimal QuotePrice { get; set; }
        public int QuantityInStock { get; set; }
        public int QuantityTransfer { get; set; }
        [StringLength(100)]

        public string UnitName { get; set; }
        public string RequestUnit { get; set; }
        public int IdWarehouseReceiving { get; set; }
        public int IdUnit { get; set; }
    }
}
