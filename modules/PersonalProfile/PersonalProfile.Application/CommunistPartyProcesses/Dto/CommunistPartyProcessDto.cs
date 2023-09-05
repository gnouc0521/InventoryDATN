using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System.ComponentModel.DataAnnotations;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.CommunistPartyProcesses.Dto
{
    public class GetAllFilter
    {
        public int? PersonId { get; set; }
    }

    [AutoMap(typeof(CommunistPartyProcess))]
    public class CommunistPartyProcessDto : EntityDto<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public int Year { get; set; }
        [Required]
        public BoolEnum PartyMemberBackground { get; set; }

        [Required]
        public BoolEnum EvaluatePartyMember { get; set; }
    }
}
