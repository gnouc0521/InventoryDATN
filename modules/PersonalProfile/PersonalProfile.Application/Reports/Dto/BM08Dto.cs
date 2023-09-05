using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class TrainningDetail
    {
        public string TrainningContent { get; set; }
        public string TrainningType { get; set; }
        public string TrainningTime { get; set; }
    }
    public class BM08Dto
    {
        public int STT { get; set; }
        public string FullName { get; set; }
        public string WorkingTitle { get; set; }
        public string Org { get; set; }
        public string Glone { get; set; }
        public List<TrainningDetail> TrainningDetails { get; set; }
     
    }
}
