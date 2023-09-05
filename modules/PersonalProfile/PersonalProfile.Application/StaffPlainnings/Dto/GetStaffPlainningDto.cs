using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto;
using System;

namespace bbk.netcore.mdl.PersonalProfile.Application.StaffPlainnings.Dto
{
    public class GetAllFilter
    {
        public int? PersonId { get; set; }
    }
    public class GetStaffPlainningDto : EntityDto<int>
    {
        public GetStaffPlainningDto() { }
        public GetStaffPlainningDto(StaffPlainning staffPlainning,DocListDto doc)
        {
            DecisionNumber = doc.DecisionNumber;
            IssuedDate = doc.IssuedDate;
            Id = staffPlainning.Id;
            PersonId = staffPlainning.PersonId;
            IssuedLevels = staffPlainning.IssuedLevels;
            WorkingTitle = staffPlainning.WorkingTitle;
            WorkingTitle1 = staffPlainning.WorkingTitle1;
            WorkingTitle2 = staffPlainning.WorkingTitle2;
            FromDate = staffPlainning.FromDate;
            ToDate = staffPlainning.ToDate;
        }
        public int PersonId { get; set; }
        public string DecisionNumber { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedLevels { get; set; }
        public string WorkingTitle { get; set; }
        public string WorkingTitle1 { get; set; }
        public string WorkingTitle2 { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
    }
}
