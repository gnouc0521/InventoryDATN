using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppQuotesRelationship", Schema = netcoreConsts.SchemaName)]
    public class QuoteRelationship : FullAuditedEntity<long>
    {
        public long QuoteSynthesiseId { get; set; }
        public long QuoteId { get; set; }


    }
}
