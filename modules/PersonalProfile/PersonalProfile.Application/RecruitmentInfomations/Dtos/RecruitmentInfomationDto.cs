using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.RecruitmentInfomations.Dtos
{
    [AutoMap(typeof(RecruitmentInfomation))]
    public class RecruitmentInfomationDto : EntityDto
    {
        public BoolConst RegistrationForm { get; set; }
        public BoolConst CertifiedBackground { get; set; }
        public BoolConst CopyOfBirthCertificate { get; set; }
        public BoolConst HealthCertificate { get; set; }
        public string PreferredCertificate { get; set; }
        public BoolConst NotDisciplined { get; set; }
        [Required]
        public string Expertise { get; set; }
        
        public string OtherLanguage { get; set; }
      
        public string InfomationTechnology { get; set; }
        [Required]
        public DateTime TimeElectNotice { get; set; }
        [Required]
        public DateTime TimeDecision { get; set; }
        [Required]
        public string WorkUnit { get; set; }
        [Required]
        public DateTime TimeGetJob { get; set; }
        [Required]
        public string RegulationsGuideProbation { get; set; }
        public BoolConst RegimeOfApprentice { get; set; }      
        public string RegimeOfApprenticeInstructor { get; set; }
        public BoolConst ExemptProbationary { get; set; }
        [Required]
        public int ProfileStaffId { get; set; }
        [Required]
        public long DocumentId { get; set; }
     
    }
}
