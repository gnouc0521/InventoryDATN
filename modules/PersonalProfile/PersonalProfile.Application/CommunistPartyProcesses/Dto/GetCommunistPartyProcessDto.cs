using Abp.Application.Services.Dto;

namespace bbk.netcore.mdl.PersonalProfile.Application.CommunistPartyProcesses.Dto
{

    public class GetCommunistPartyProcessDto
    {
        public int Id { get; set; }
        public int Year{ get; set; }

        public string PartyMemberBackground { get; set; }

        public string EvaluatePartyMember { get; set; }
    }
}
