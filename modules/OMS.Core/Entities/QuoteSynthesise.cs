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
    [Table("AppQuotesSynthesise", Schema = netcoreConsts.SchemaName)]
    public class QuoteSynthesise : FullAuditedEntity<long>
    {
        public string Code { get; set; }
        public DateTime QuoteSynthesiseDate { get; set; }
        public QuoteEnum.SyntheticQuote Status { get; set; }
        public string Comment { get; set; }
    }
}
