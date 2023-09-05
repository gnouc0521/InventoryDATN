using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppPurchaseAssignment", Schema = netcoreConsts.SchemaName)]
    public class PurchaseAssignment : FullAuditedEntity 
    {
        public long PurchasesSynthesiseId { get; set; }
        public virtual PurchasesSynthesise purchasesRequest { get; set; }
     
        public long UserId { get; set; }

        public  long ItemId { get; set; }

        public PurchasesRequestEnum.MyworkStatus Status { get; set; }

        public PurchasesRequestEnum.GetPriceSupkStatus GetPriceStatus { get; set; }
    }
}
