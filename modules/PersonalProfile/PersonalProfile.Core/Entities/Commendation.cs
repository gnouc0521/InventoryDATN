using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppCommendations", Schema = netcoreConsts.SchemaName)]
    public class Commendation : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public long DocumentId { get; set; }

        [Required]
        public int CommendationYear { get; set; }

        [Required]
        public int CommendationTitleId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public ProfileStaff ProfileStaff { get; set; }   

        [ForeignKey(nameof(CommendationTitleId))]
        public Category CommendationTitle { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document Document{ get; set; }
    }
}
