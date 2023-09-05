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
    [Table("AppOrder", Schema = netcoreConsts.SchemaName)]
    public class Order : FullAuditedEntity<long>
    {
        public string Code { get; set; }
        public PurchasesRequestEnum.OrderStatus OrderStatus { get; set; }
        public long UserId  { get; set; }
        public int ContractId  { get; set; }
        public long QuoteId  { get; set; }
        public bool StatusCreateYCNK { get; set; }
    }
}
