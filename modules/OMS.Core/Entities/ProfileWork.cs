using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppProfileWorks", Schema = netcoreConsts.SchemaName)]
    public class ProfileWork: FullAuditedEntity, ISoftDelete
    {
        [Required]
        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string Title { get; set; }   

        public int? ParentId { get; set; }

        public int Order { get; set; }

        public int UserId { get; set; }

        public int WorkProfileLevel { get; set; }
        public ICollection<Work> Work { get; set; }

    }
}
