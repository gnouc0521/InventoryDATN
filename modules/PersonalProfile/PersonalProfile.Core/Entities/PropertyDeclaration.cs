using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppPropertyDeclarations", Schema = netcoreConsts.SchemaName)]
    public class PropertyDeclaration : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public int Year { get; set; }

        public PropertyDeclarationBoolConst? IsExist { get; set; }

        public string FileUrl { get; set; }

        public string FilePath { get; set; }

        [ForeignKey(nameof(PersonId))]
        public ProfileStaff ProfileStaff { get; set; }
    }
}
