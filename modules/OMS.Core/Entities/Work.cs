using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;
using bbk.netcore.mdl.OMS.Core.Enums;
using bbk.netcore.Authorization.Users;
using Abp.Authorization.Users;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppWorks", Schema = netcoreConsts.SchemaName)]
    public class Work : FullAuditedEntity, ISoftDelete
    {
        [Required]
        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string Title { get; set; }
        public string Description { get; set; }
      
        public int  IdWorkGroup { get; set; }
        public virtual WorkGroup workGroup { get; set; }

        public int IdProfileWork { get; set; }
        public virtual ProfileWork profileWork { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public DateTime CompletionTime { get; set; }
        public WorkEnum.Status Status { get; set; }

        public WorkEnum.Priority priority { get; set; }

        public WorkEnum.JobLabel jobLabel { get; set; } 

        [Required]
        public int HostId { get; set; }
        public string FileUrl { get; set; }

        public string FilePath { get; set; }
      
    }
}
