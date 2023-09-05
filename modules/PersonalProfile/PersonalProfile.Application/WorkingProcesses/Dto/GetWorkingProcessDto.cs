using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Categories.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.Organizations.Dto;
using System;

namespace bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses.Dto
{
    public class GetAllWorkingProcessFilter
    {
        public int PersonId { get; set; }
    }

    public class GetWorkingProcessDto : EntityDto<int>
    {
        public GetWorkingProcessDto() { }
        public GetWorkingProcessDto(WorkingProcess w, CategoryDto decisionMaker, CategoryDto workingTitle, DocListDto document)
        {
            if(document != null)
            {
                DecisionNumber = document.DecisionNumber;
                IssuedDate = document.IssuedDate;
            }
            else
            {
                DecisionNumber = string.Empty;
                IssuedDate = (DateTime?) null;
            }

            Id = w.Id;
            PersonId = w.PersonId;
            OrganName = w.OtherOrg;
            DepartmentName = w.DepartmentName;
            FromDate = w.FromDate;
            ToDate = w.ToDate;
            JobPosition = w.JobPosition;
            DecisionMaker = decisionMaker != null ? decisionMaker.Title : "";
            WorkingTitle = workingTitle.Title;
            TypeOfChange = w.TypeOfChangeString;
        }
        public int PersonId { get; set; }
        public string JobPosition { get; set; }
        public string WorkingTitle{ get; set; }
        public string OrganName { get; set; }
        public string DepartmentName { get; set; }
        public string DecisionNumber { get; set; }
        public string DecisionMaker { get; set; }
        public string TypeOfChange { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
