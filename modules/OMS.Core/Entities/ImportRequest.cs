using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppImportRequest", Schema = netcoreConsts.SchemaName)]
    public class ImportRequest : FullAuditedEntity
    {
        [StringLength(50)]
        public string Code { get; set; }
        public int WarehouseDestinationId { get; set; }
    public int RequestType { get; set; }
    public long SupplierId { get; set; }
    public long SubsidiaryId { get; set; }
    public decimal VAT { get; set; }

    public decimal TotalVal { get; set; }
    public decimal ToatlAmount { get; set; }
    public ImportResquestEnum.ImportResquestStatus ImportStatus { get; set; }
    [StringLength(1000)]
    public string Remark { get; set; }
    public DateTime RequestDate { get; set; }
        public long TransferId { get; set; }
        public  long ImportRequestSubsidiaryId { get; set; }
        [StringLength(150)]
        public string ShipperName { get; set; }

        [StringLength(150)]
        public string ShipperPhone { get; set; }
    }
}
