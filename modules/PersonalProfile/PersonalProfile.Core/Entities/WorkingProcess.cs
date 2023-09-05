using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppWorkingProcesses", Schema = netcoreConsts.SchemaName)]
    public class WorkingProcess : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }

        public long? DocumentId { get; set; }

        [StringLength(256)]
        public string JobPosition { get; set; }

        [Required]
        public int WorkingTitleId { get; set; }

        public int? DecisionMakerId { get; set; }

        public long? OrgId { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string OtherOrg { get; set; }

        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string DepartmentName { get; set; }

        [StringLength(100)]
        public string TypeOfChangeString { get; set; }

        public int? TypeOfChangeId { get; set; }

        [Required]
        [Column(TypeName ="date")]
        public DateTime FromDate { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [ForeignKey(nameof(PersonId))]
        public ProfileStaff ProfileStaff { get; set; }     

        [ForeignKey(nameof(WorkingTitleId))]
        public Category WorkingTitle { get; set; }        
        
        [ForeignKey(nameof(DecisionMakerId))]
        public Category DecisionMaker { get; set; }        
        
        [ForeignKey(nameof(TypeOfChangeId))]
        public Category TypeOfChange { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document Document { get; set; }

        [ForeignKey(nameof(OrgId))]
        public OrganizationUnit OrganizationUnit { get; set; }
    }
}
