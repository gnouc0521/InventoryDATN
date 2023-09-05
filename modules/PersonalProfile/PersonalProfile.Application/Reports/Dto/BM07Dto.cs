using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM07Dto
    {
        public int STT { get; set; }
        public string FullName { get; set; }
        public string OldWorkingTitleAndOrg { get; set; }
        public string TimeHoldWorkingTitle{ get; set; }
        public DateTime IssuedDate { get; set; }
        public string DecisionMaker { get; set; }
        public string  WorkingTitleAndOrg { get; set; }
    }
}
