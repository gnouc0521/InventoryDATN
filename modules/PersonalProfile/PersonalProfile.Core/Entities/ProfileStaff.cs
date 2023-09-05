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
    [Table("AppProfileStaffs", Schema = netcoreConsts.SchemaName)]
    public class ProfileStaff : FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string FullName { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string OtherName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string ProvinceOfBirth { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string DistrictOfBirth { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string VillageOfBirth { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string ProvinceOfNativePlace { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string DistrictOfNativePlace { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string VillageOfNativePlace { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string NativePlaceDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string Ethnic { get; set; }

        [Required]
        [StringLength(50)]
        public string Religion { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string PermanentPlace { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string ResidentialPlace { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Occupation { get; set; }

        [Required]
        public DateTime RecruitmentDate { get; set; }

        [Required]
        public string RecruitmentUnit { get; set; }

        [Required]
        public int CurrentPosition { get; set; }

        public string ConcurrentPosition { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string KeyResponsibilities { get; set; }

        [Required]
        public int CivilServantSector { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string CivilServantSectorCode { get; set; }

        [Required]
        public int PayRate { get; set; }

        [Required]
        public DateTime PayDay { get; set; }

        public string PositionAllowance { get; set; }

        public string OtherAllowance { get; set; }

        [Required]
        [StringLength(30)]
        public string HighSchoolEducationalLevel { get; set; }

        [Required]
        public int HighestAcademicLevel { get; set; }

        public int? AcademicTitle { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Specialized { get; set; }

        [Required]
        public int PoliticsTheoReticalLevel { get; set; }


        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string ForeignLanguage { get; set; }

        [Required]
        public string StateManagement { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string InfomationTechnology { get; set; }

        public DateTime? DateOfJoiningInVNCommunistParty { get; set; }

        public DateTime? OfficialDateOfJoiningInVNCommunistParty { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string DateOfJoiningSocialPoliticalAssosiations { get; set; }

        public DateTime? DateOfEnlistment { get; set; }

        public DateTime? DateOfDisCharge { get; set; }

        [StringLength(50)]
        public string HighestArmyRank { get; set; }

        [StringLength(100)]
        public string HighestAwardedTitle { get; set; }

        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string WorkStrength { get; set; }

        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string Awards { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Discipline { get; set; }

        [StringLength(50)]
        public string HealthStatus { get; set; }

        [StringLength(20)]
        public string Height { get; set; }

        [StringLength(20)]
        public string Weight { get; set; }

        [StringLength(10)]
        public string BloodGroup { get; set; }

        public int? WoundedSoldierRank { get; set; }

        [StringLength(100)]
        public string SonOfFamilyPolicy { get; set; }

        [Required]
        [StringLength(30)]
        public string IdentityCardNo { get; set; }

        [Required]
        public DateTime DateOfIssuanceIdentityCard { get; set; }

        [Required]
        [StringLength(30)]
        public string SocialInsuranceBookNo { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Imprisoned { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string OrganizationAbroad { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string RelativesAbroad { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Note { get; set; }

        public BoolEnum? StaffBackground { get; set; }
                       
        public BoolEnum? CurriculumVitae { get; set; }
                       
        public BoolEnum? AdditionalStaffBackground { get; set; }
                       
        public BoolEnum? BriefBiography { get; set; }
                       
        public BoolEnum? BirthCertificate { get; set; }
                     
        public BoolEnum? HealthCertificate { get; set; }

        public BoolEnum? PersonalIdentityDcuments { get; set; }
                    
        public BoolEnum? TrainingQualificationCV { get; set; }
                    
        public BoolEnum? RecruitmentDecision { get; set; }
                
        public BoolEnum? DocumentsAppointingPosition { get; set; }
             
        public BoolEnum? SelfAssessment { get; set; }
             
        public BoolEnum? EvaluationComment { get; set; }
               
        //public BoolEnum? PropertyDeclaration { get; set; }
                     
        public BoolEnum? HandlingDocument { get; set; }
                       
        public BoolEnum? WorkProcessDocument { get; set; }
                      
        public BoolEnum? RequestFormToResearchRecord { get; set; }
                       
        public BoolEnum? DocumentObjective { get; set; }
                       
        public BoolEnum? DocumentClipCover { get; set; }
                       
        public BoolEnum? ProfileCover { get; set; }
        
        public string ImageURL { get; set; }
       
        public string ImagePath { get; set; }
    }
}
