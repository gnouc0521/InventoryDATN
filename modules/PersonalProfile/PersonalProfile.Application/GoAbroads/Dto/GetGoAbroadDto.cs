using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto;
using System;

namespace bbk.netcore.mdl.PersonalProfile.Application.GoAbroads.Dto
{
    public class GetAllFilter
    {
        public int? PersonId { get; set; }
    }
    public class GetGoAbroadDto : EntityDto<int>
    {
        public GetGoAbroadDto() { }
        public GetGoAbroadDto(GoAbroad goAbroad,DocListDto doc)
        {
            DecisionNumber = doc.DecisionNumber;
            IssuedDate = doc.IssuedDate;
            Id = goAbroad.Id;
            PersonId = goAbroad.PersonId;
            FromDate = goAbroad.FromDate;
            ToDate = goAbroad.ToDate;
            Summary = goAbroad.Summary;
            Location = goAbroad.Location;
        }
        public int PersonId { get; set; }
        public string DecisionNumber { get; set; }
        public DateTime IssuedDate { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
    }
}
