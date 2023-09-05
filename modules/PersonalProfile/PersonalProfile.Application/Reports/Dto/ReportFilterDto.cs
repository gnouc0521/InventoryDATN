using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class ReportFilterDto
    {
        public ReportEnum.ReportTypeEnum ReportEnum { get; set; }
        public int? OrgId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
