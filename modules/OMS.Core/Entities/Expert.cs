using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppExperts", Schema = netcoreConsts.SchemaName)]
    public class Expert : FullAuditedEntity, ISoftDelete
    {
        [StringLength(50)]
        public string ExpertCode { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public long UserId { get; set; }
        public virtual AbpUserBase User { get; set; }
    }
}
