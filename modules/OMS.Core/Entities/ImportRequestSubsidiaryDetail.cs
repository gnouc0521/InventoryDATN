using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppImportRequestSubsidiaryDetail", Schema = netcoreConsts.SchemaName)]
    public class ImportRequestSubsidiaryDetail : FullAuditedEntity
    {
        public int ImportRequestSubsidiaryId { get; set; }
        public virtual ImportRequestSubsidiary ImportRequestSubsidiary { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public DateTime TimeNeeded { get; set; }
    }
}
