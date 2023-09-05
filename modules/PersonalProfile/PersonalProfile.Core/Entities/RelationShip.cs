using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppRelationShips", Schema = netcoreConsts.SchemaName)]
    public class RelationShip : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        public int? YearBirth { get; set; }

        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string Info { get; set; }

        [Required]
        [StringLength(50)]
        public string RelationName { get; set; }

        [Required]
        public RelationType Type { get; set; }

        [ForeignKey(nameof(PersonId))]
        public ProfileStaff ProfileStaff { get; set; }
    }
}
