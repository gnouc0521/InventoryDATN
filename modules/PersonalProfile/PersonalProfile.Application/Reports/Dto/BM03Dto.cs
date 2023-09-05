using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM03Dto
    {
        public int STT { get; set; }

        public string FullName { get; set; }

        public string ServantCode { get; set; }

        public DateTime? IssuedDate { get; set; }

        public MeetQualificationRequirement MeetQualificationRequirement { get; set; }

        public string Note { get; set; }
    }


    public class MeetQualificationRequirement
    {
        public string Specialize { get; set; }

        public string StateManagement { get; set; }

        public string ForeignLanguage { get; set; }

        public string InformationTechnology { get; set; }

        public string PoliticsTheoReticalLevel { get; set; }

        public string ProjectHasCreativity { get; set; }

        public string TimeKeepOldServant { get; set; }

        public string FosteringTraining { get; set; }

        public string Note { get; set; }
    }
}
