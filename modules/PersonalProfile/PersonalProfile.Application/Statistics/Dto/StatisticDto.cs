using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Statistics.Dto
{
    public class StatisticDto : EntityDto
    {
        public int STT { get; set; }

        public string FullName { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string HighestAcademicLevel { get; set; }

        public string Specialized { get; set; }

        public string Position { get; set; }

        public DateTime? RecruitmentDate { get; set; }

        public DateTime? TimeIncreaseSalary { get; set; }

        public string CoefficientsSalary { get; set; }

        public string CivilServant { get; set; }

        public string CivilServantCode { get; set; }

        public string NativePlace { get; set; }

        public string PositionAllowance { get; set; }

        public string OtherAllowance { get; set; }
    }

    public class GetStatisticInput
    {
        public long? OrgId { get; set; }

        public Gender? Gender { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public string Ethnic { get; set; }

        public int? TypeServant { get; set; }

        public int?[] AcademicTitle { get; set; }

        public DateTime? SalaryRaiseTime { get; set; }

        public int?[] Position { get; set; }

        public DateTime? FromRecruitmentDate { get; set; }

        public DateTime? ToRecruitmentDate { get; set; }

        public int?[] PoliticsTheoReticalLevel { get; set; }

        public bool? IsPartyMember { get; set; }

        public AllowanceType?[] Allowance { get; set; }

        public int? MaxResultCount { get; set; }

        public int? SkipCount { get; set; }
    }
}
