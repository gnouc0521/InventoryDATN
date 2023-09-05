using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppStaffPlainnings", Schema = netcoreConsts.SchemaName)]
    public class StaffPlainning : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public long DocumentId { get; set; }

        [Required]
        [StringLength(100)]
        public string IssuedLevels { get; set; }

        [Required]
        [StringLength(100)]
        public string WorkingTitle { get; set; }

        public string WorkingTitle1 { get; set; }

        public string WorkingTitle2 { get; set; }

        [Required]
        public int FromDate { get; set; }

        [Required]
        public int ToDate { get; set; }

        [ForeignKey(nameof(PersonId))]
        public ProfileStaff Person { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document Document { get; set; }
    }
}
