using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM01Dto
    {
        public int STT { get; set; }

        public string OrganizationUnit { get; set; }

        public Child01 PresentTo { get; set; }

        public Child01 PayrollIncreaseOrDecrease { get; set; }

        public int? Total { get; set; }

        public int? OtherConstractPresentTo { get; set; }
    }

    public class Child01
    {
        public int? Total { get; set; }

        public int? PayrollByLeadershipPositions { get; set; }

        public int? PayrollByProfessionalTitle { get; set; }

        public int? Constract { get; set; }
    }
}
