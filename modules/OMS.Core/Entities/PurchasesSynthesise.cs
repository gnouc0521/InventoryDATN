using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppPurchasesSynthesise", Schema = netcoreConsts.SchemaName)]
    public class PurchasesSynthesise : FullAuditedEntity<long>
    {
        public string PurchasesSynthesiseCode { get; set; }

        public bool StatusAssignment { get; set; }

        public string Comment { get; set; }
        public PurchasesRequestEnum.YCNK PurchasesRequestStatus { get; set; }
    }
}
