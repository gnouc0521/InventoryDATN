using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM04Dto
    {
        public int STT { get; set; }
        public string FullName { get; set; }
        public string OldJobPosition { get; set; }
        public string TimeHoldOldJobPosition { get; set; }
        public string NewWorkingTitle { get; set; }
        public string SecondedTime { get; set; }
        public int? DecisionYear { get; set; }
        public string DecisionMaker { get; set; }
        public string Glone { get; set; }
        public string OldAndNewWorkiingTitle { get; set; }
        public string Allowance { get; set; }
    }
}
