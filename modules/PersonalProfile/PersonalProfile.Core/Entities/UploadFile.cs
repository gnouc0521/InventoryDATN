using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppUploadFiles", Schema = netcoreConsts.SchemaName)]
    public class UploadFile : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public string FileName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string FileUrl { get; set; }
        [ForeignKey(nameof(PersonId))]
        public ProfileStaff ProfileStaff { get; set; }
       
    }
}