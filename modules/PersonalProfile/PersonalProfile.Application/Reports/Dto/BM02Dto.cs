using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class Bm02Dto
    {
        public int STT { get; set; }
        public string FullName { get; set; }
        public DateTime? MaleDateTime { get; set; }
        public DateTime? FeMaleDateTime { get; set; }
        public DateTime? Year { get; set; }
        public CoefficientsSalaryDetail currentCoefficientsSalaryDetail { get; set; }
        public CoefficientsSalaryDetail oldCoefficientsSalaryDetail { get; set; }
    }
}
