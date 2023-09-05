using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM09Dto
    {
        public int STT { get; set; }

        public string FullName { get; set; }

        public Evaluate Evaluate1 { get; set; }

        public Evaluate Evaluate2 { get; set; }
    }

    public class Evaluate
    {
        public string Position { get; set; }

        public string SelfAssessment { get; set; }

        public string LeaderAssessment { get; set; }

        public string GroupAssessment { get; set; }

        public string CmpetentPersonsAssessment { get; set; }

        public string Result { get; set; }
    }
}
