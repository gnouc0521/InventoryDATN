using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppTransfers", Schema = netcoreConsts.SchemaName)]
    public class Transfer : FullAuditedEntity<long>
    {
        [StringLength(50)]
        public string TransferCode { get; set; }
        public int IdWarehouseExport { get; set; }
        public DateTime BrowsingTime { get; set; }
        public string TransferNote { get; set; }
        public TransferEnum.TransferStatus Status { get; set; }
        public ContractEnum.ExportStatus ExportStatus { get; set; }
        public bool StatusImportPrice { get; set; }
        public string CommentText { get; set; }

    }
}
