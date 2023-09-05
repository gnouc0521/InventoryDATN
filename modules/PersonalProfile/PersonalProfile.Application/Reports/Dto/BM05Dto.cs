using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM05Dto
    {
        public int STT { get; set; }

        public string FullName { get; set; }

        public string Male { get; set; }

        public string FeMale { get; set; }

        public string Position { get; set; }

        public DateTime? DecisionAppointDate { get; set; }

        public DateTime? DecisionReAppointDate { get; set; }

        public string DecisionMaker { get; set; }

        public string CurriculumVitae { get; set; }

        public string PropertyDeclaration { get; set; }

        public string Qualification { get; set; }

        public string PoliticsTheoReticalLevel { get; set; }

        public string StateManagement { get; set; }

        public string ForeignLanguage { get; set; }

        public string InformationTechnology { get; set; }
    }
}
