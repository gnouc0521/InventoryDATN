using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.SalaryProcesses.Dtos
{
    [AutoMap(typeof(SalaryProcess))]
    public class SalaryProcessDto : Entity
    {
        [Required]
        public int PersonId { get; set; }
        [Required]
        public string DecisionNumber { get; set; }
        [Required]
        public DateTime IssuedTime { get; set; }
        [Required]
        public DateTime SalaryIncreaseTime { get; set; }
        [Required]
        public string Glone { get; set; }
        [Required]
        public string GloneCode { get; set; }
        [Required]
        public string PayRate { get; set; }
        [Required]
        public string CoefficientsSalary { get; set; }
        public DateTime NextSalaryIncreaseTime { get; set; }
        public string LeadershipPositionAllowance { get; set; }
        public string ToxicAllowance { get; set; }
        public string AreaAllowance { get; set; }
        public string ResponsibilityAllowance { get; set; }
        public string MobileAllowance { get; set; }
    }
}