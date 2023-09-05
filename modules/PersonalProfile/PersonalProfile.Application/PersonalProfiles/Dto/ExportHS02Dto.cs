using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.RelationShips.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses.Dtos;
using bbk.netcore.mdl.PersonalProfile.Application.TrainningInfos.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto
{
    public class ExportHS02Dto : EntityDto
    {
        public string OrganizationUnit { get; set; }

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

        public string CurrentPosition { get; set; }

        public string ConcurrentPosition { get; set; }

        public string KeyResponsibilities { get; set; }

        public string CivilServantSector { get; set; }

        public string CivilServantSectorCode { get; set; }

        public string PayRate { get; set; }

        public string Coefficientssalary { get; set; }

        public DateTime PayDay { get; set; }

        public string PositionAllowance { get; set; }

        public string OtherAllowance { get; set; }

        public string HighSchoolEducationalLevel { get; set; }

        public string HighestAcademicLevel { get; set; }

        public string AcademicTitle { get; set; }

        public string Specialized { get; set; }

        public string PoliticsTheoReticalLevel { get; set; }

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

        public string WoundedSoldierRank { get; set; }

        public string SonOfFamilyPolicy { get; set; }

        public string IdentityCardNo { get; set; }

        public DateTime DateOfIssuanceIdentityCard { get; set; }

        public string SocialInsuranceBookNo { get; set; }

        public string Imprisoned { get; set; }

        public string OrganizationAbroad { get; set; }

        public string RelativesAbroad { get; set; }

        public string Evaluate { get; set; }

        public List<GetTrainningInfoDto> TrainningInfos { get; set; }

        public List<GetWorkingProcessDto> WorkingProcesses { get; set; }

        public List<RelationShipDto> RelationSelf { get; set; }

        public List<RelationShipDto> RelationOther { get; set; }

        public List<SalaryProcessDto> SalaryProcesses { get; set; }
    }
}
