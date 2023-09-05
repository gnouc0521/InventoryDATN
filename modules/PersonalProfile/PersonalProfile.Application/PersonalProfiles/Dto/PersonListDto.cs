using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto
{
    public class PersonListDto : EntityDto
    {
        public string FullName { get; set; }

        public string Position { get; set; }

        public int? Age { get; set; }

        public DateTime? StartedDate { get; set; }

        public string CivilServant { get; set; }

        public string CoefficientsSalary { get; set; }
    }

    public class GetAllPersonInput
    {
        public string Keyword { get; set; }

        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; }

        public long? OrganizationUnitId { get; set; }
    }
}
