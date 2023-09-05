using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppRecruitmentInfomations", Schema = netcoreConsts.SchemaName)]
    public class RecruitmentInfomation : Entity
    {
       
        public BoolConst RegistrationForm { get; set; }
        
        public BoolConst CertifiedBackground { get; set; }
       
        public BoolConst CopyOfBirthCertificate { get; set; }
        
        public BoolConst HealthCertificate { get; set; }
        
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string PreferredCertificate { get; set; }
        
        public BoolConst NotDisciplined { get; set; } 
        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Expertise { get; set; }
        
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string OtherLanguage { get; set; }
        
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string InfomationTechnology { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime TimeElectNotice { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime TimeDecision { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string WorkUnit { get; set; }

        [Column(TypeName = "date")]
        public DateTime TimeGetJob { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string RegulationsGuideProbation { get; set; }

        public BoolConst RegimeOfApprentice { get; set; }
        
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string RegimeOfApprenticeInstructor { get; set; }

        public BoolConst ExemptProbationary { get; set; }
        [Required]
        [ForeignKey(nameof(ProfileStaffId))]
        public ProfileStaff ProfileStaff { get; set; }
        [Required]
        public int ProfileStaffId { get; set; }
        [Required]
        public long DocumentId { get; set; }
       

    }
}

