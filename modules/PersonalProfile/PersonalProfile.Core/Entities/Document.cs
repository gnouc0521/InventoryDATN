using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using Abp.Domain.Entities;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppDocuments", Schema = netcoreConsts.SchemaName)]
    public class Document : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string DecisionNumber { get; set; }

        public string Description { get; set; }

        public string DocumentTypeName { get; set; }

        public string IssuedOrganization { get; set; }
        [Required]
        public int DocumentCategoryTypeId { get; set; }

        public DateTime IssuedDate { get; set; }

        public DocumentEnum.DocumentCategoryEnum DocumentCategoryEnum { get; set; }

        public DocumentEnum.DocumentTypeEnum? DocumentTypeEnum { get; set; }

        public string FileUrl { get; set; }

        public string FilePath { get; set; }

        [ForeignKey(nameof(DocumentCategoryTypeId))]
        public Category DocumentCategoryType { get; set; }

        public ICollection<Commendation> Commendations { get; set; }

        public ICollection<WorkingProcess> WorkingProcesses{ get; set; }

        public ICollection<StaffPlainning> StaffPlainnings{ get; set; }

        public ICollection<GoAbroad> GoAbroads{ get; set; }
    }
}
