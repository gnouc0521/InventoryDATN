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
    [Table("AppDayOffs", Schema = netcoreConsts.SchemaName)]
    public class DayOff : FullAuditedEntity, ISoftDelete
    {
        [Required]
        public string Title { get; set; }
        public DayOffEnum.TypeDayOff TypeDayOff { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SumDayOff { get; set; }
        public int Order { get; set; }
    }
}
