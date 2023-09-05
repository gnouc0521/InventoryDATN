using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppGoAbroads", Schema = netcoreConsts.SchemaName)]
    public class GoAbroad : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public long DocumentId { get; set; }

        [Required]
        public int FromDate { get; set; }

        [Required]
        public int ToDate { get; set; }

        [Required]
        public string Location { get; set; }

        public string Summary { get; set; }

        [ForeignKey(nameof(PersonId))]
        public ProfileStaff Person { get; set; }


        [ForeignKey(nameof(DocumentId))]
        public Document Document { get; set; }
    }
}
