using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM12Dto
    {
        public int STT { get; set; }

        public string FullName { get; set; }

        public DateTime? Male { get; set; }

        public DateTime? FeMale { get; set; }

        public string Position { get; set; }

        public DateTime? DecisionDate { get; set; }

        public Retirement Retirement { get; set; }

        public Decision Decision { get; set; }
    }

    public class Retirement
    {
        public string ToRetirementAge { get; set; }

        public DateTime? NoticeDate { get; set; }

        public DateTime RetirementTimeInNotice { get; set; }
    }

    public class Decision
    {
        public DateTime? DecisionDate { get; set; }

        public DateTime? RetirementTime { get; set; }

        public string TitlePersonDecision { get; set; }
    }
}
