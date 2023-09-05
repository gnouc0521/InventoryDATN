using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppCommunistPartyProcesses", Schema = netcoreConsts.SchemaName)]
    public class CommunistPartyProcess : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public BoolEnum PartyMemberBackground { get; set; }

        [Required]
        public BoolEnum EvaluatePartyMember { get; set; }
    }
}
