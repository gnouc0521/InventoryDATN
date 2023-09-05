using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppCategories", Schema = netcoreConsts.SchemaName)]
    public class Category : FullAuditedEntity
    {
        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Title { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        public ICollection<TrainningInfo> Diplomas { get; set; }
        public ICollection<WorkingProcess> WorkingTitles{ get; set; }
        public ICollection<AssessedByYear> SelfAssessments { get; set; }
    }
}
