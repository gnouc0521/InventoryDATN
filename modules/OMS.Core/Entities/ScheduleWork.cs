using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppScheduleWorks", Schema = netcoreConsts.SchemaName)]
    public class ScheduleWork : FullAuditedEntity, ISoftDelete
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public string Address { get; set; }
        public int? Repeat { get; set; }
        public ScheduleWorkEnum.ColorPriority Color { get; set; }
    }
}
