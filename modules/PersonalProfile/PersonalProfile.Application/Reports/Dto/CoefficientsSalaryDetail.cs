using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class CoefficientsSalaryDetail
    {
        public string WorkingTitle { get; set; }
        public string OrganName { get; set; }
        public string JobPosition { get; set; }
        public string Glone { get; set; }
        public string CoefficientsSalary { get; set; }
        public string PayRate { get; set; }
        public DateTime FromDate { get; set; }
        public string SeniorityAllowance { get; set; }
        public string PositionAllowance { get; set; }
        public string OtherAllowance { get; set; }
        public string DifferenceCoefficient { get; set; }

        public string Note { get; set; }
    }
}
