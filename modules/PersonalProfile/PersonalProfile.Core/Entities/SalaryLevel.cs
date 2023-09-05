using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppSalaryLevels", Schema = netcoreConsts.SchemaName)]
    public class SalaryLevel: AuditedEntity<int>
   {
        [Required]
        [StringLength(20)]
        public string Level { get; set; }

        [Required]
        [StringLength(20)]
        public string CoefficientsSalary { get; set; }

        public CivilServantGroup Group { get; set; }
    }
}
