using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto
{
    [AutoMapFrom(typeof(ProfileStaff))]
    public class PersonalProfileDto : EntityDto
    {
        public long? OrganizationUnitId { get; set; }

        public string FullName { get; set; }

        public string OtherName { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string ProvinceOfBirth { get; set; }

        public string DistrictOfBirth { get; set; }

        public string VillageOfBirth { get; set; }

        public string ProvinceOfNativePlace { get; set; }

        public string DistrictOfNativePlace { get; set; }

        public string VillageOfNativePlace { get; set; }

        public string NativePlaceDescription { get; set; }

        public string Ethnic { get; set; }

        public string Religion { get; set; }

        public string PermanentPlace { get; set; }

        public string ResidentialPlace { get; set; }

        public string Occupation { get; set; }

        public DateTime RecruitmentDate { get; set; }

        public string RecruitmentUnit { get; set; }

        public int CurrentPosition { get; set; }

        public string ConcurrentPosition { get; set; }

        public string KeyResponsibilities { get; set; }

        public int CivilServantSector { get; set; }

        public string CivilServantSectorCode { get; set; }

        public int PayRate { get; set; }

        public DateTime PayDay { get; set; }

        public string PositionAllowance { get; set; }

        public string OtherAllowance { get; set; }

        public string HighSchoolEducationalLevel { get; set; }

        public int HighestAcademicLevel { get; set; }

        public int? AcademicTitle { get; set; }

        public string Specialized { get; set; }

        public int PoliticsTheoReticalLevel { get; set; }

        public string ForeignLanguage { get; set; }

        public string StateManagement { get; set; }

        public string InfomationTechnology { get; set; }

        public DateTime? DateOfJoiningInVNCommunistParty { get; set; }

        public DateTime? OfficialDateOfJoiningInVNCommunistParty { get; set; }

        public string DateOfJoiningSocialPoliticalAssosiations { get; set; }

        public DateTime? DateOfEnlistment { get; set; }

        public DateTime? DateOfDisCharge { get; set; }

        public string HighestArmyRank { get; set; }

        public string HighestAwardedTitle { get; set; }

        public string WorkStrength { get; set; }

        public string Awards { get; set; }

        public string Discipline { get; set; }

        public string HealthStatus { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string BloodGroup { get; set; }

        public int? WoundedSoldierRank { get; set; }

        public string SonOfFamilyPolicy { get; set; }

        public string IdentityCardNo { get; set; }

        public DateTime DateOfIssuanceIdentityCard { get; set; }

        public string SocialInsuranceBookNo { get; set; }

        public string Imprisoned { get; set; }

        public string OrganizationAbroad { get; set; }

        public string RelativesAbroad { get; set; }

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
