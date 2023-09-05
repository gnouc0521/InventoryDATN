using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppTrainningInfos", Schema = netcoreConsts.SchemaName)]
    public class TrainningInfo: AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public int TrainningTypeId { get; set; }

        [Required]
        public int DiplomaId { get; set; }

        public string FileUrl{ get; set; }

        public string FilePath { get; set; }

        [Required]
        [StringLength(255)]
        public string SchoolName { get; set; }

        [Required]
        [StringLength(255)]
        public string MajoringName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        [ForeignKey(nameof(TrainningTypeId))]
        public Category TrainningType { get; set; }

        [ForeignKey(nameof(PersonId))]
        public ProfileStaff Person { get; set; }

        [ForeignKey(nameof(DiplomaId))]
        public Category Diploma { get; set; }
    }
}
