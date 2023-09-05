using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{

    [Table("AppUsersWorks", Schema = netcoreConsts.SchemaName)]
    public class UserWork : AuditedEntity
    {
        public long UserId { get; set; }
        public int WorkId { get; set; }
        public virtual Work Work { get; set; }
        public WorkEnum.Status Status { get; set; }
        public WorkEnum.OwnerStatusEnum OwnerStatus { get; set; }


    }
}
