using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.OMS.Core.Enums.ExportEnums;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppExportRequest", Schema = netcoreConsts.SchemaName)]
    public class ExportRequest : FullAuditedEntity<long>
    {
        [StringLength(50)]
        public string Code { get; set; }
        public int WarehouseDestinationId { get; set; }
        public ExportEnums.ExportType RequestType { get; set; }
        public long SupplierId { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalVal { get; set; }
        public decimal TotalAmount { get; set; }
        public ExportEnums.ExportStatus Status { get; set; }
        public ExportEnums.Export ExportStatus { get; set; }
        [StringLength(1000)]
        public string Remark { get; set; }
        public DateTime RequestDate { get; set; }

        [StringLength(150)]
        public string  Phone { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string CodeRequirement { get; set; }
        public long SubsidiaryId { get; set; }
        public long TransferId { get; set; }
        [StringLength(1000)]
        public string Comment { get; set; }
        public long IdWarehouseReceiving { get; set; }
        public string CodeTransfer { get; set; }

    }
}
