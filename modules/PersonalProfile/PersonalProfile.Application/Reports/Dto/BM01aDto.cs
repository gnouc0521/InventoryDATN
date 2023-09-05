using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM01aDto
    {
        public int STT { get; set; }

        public string FullName { get; set; }

        public string OrganizationUnit { get; set; }

        public string DateNumber { get; set; }

        public DateTime? ConstractDuration { get; set; }

        public string Work { get; set; }

        public string TitleOfConstractSigner { get; set; }

        public int? TotalContract { get; set; }
    }
}
