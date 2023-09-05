using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppAssignments", Schema = netcoreConsts.SchemaName)]
    public class Assignment : FullAuditedEntity, ISoftDelete
    {
        public int UserId { get; set; }
        public long ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Items Items { get; set; }
    }
}
