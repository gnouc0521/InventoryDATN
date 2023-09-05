using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;
using bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto;

namespace bbk.netcore.mdl.PersonalProfile.Application.Commendations.Dto
{
    public class GetAllFilter
    {
        public int? PersonId { get; set; }

        public CategoryType? Type { get; set; }
    }

    public class GetCommendationDto: EntityDto<int>
    {
        public GetCommendationDto() { }
        public GetCommendationDto(Commendation commendation, DocListDto doc)
        {
            Id = commendation.Id;
            PersonId = commendation.PersonId;
            DecisionNumber = doc.DecisionNumber;
            CommendationYear = commendation.CommendationYear;
            CommendationTitle = commendation.CommendationTitle.Title;
        }
        public int PersonId { get; set; }
        public int CommendationYear { get; set; }
        public string DecisionNumber { get; set; }
        public string CommendationTitle { get; set; }
    }
}
