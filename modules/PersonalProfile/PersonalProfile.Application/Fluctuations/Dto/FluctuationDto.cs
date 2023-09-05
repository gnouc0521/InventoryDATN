using System;
using System.ComponentModel.DataAnnotations;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Fluctuations.Dto
{
    public class FluctuationDto
    {
        public string FullName { get; set; }

        public string WorkingTitle { get; set; }

        public string Organ { get; set; }

        public int DocumentId { get; set; }

        public string DecisionNumber { get; set; }

        public DateTime ToDate { get; set; }

        public FluctuationEnum FluctuationType { get; set; }
        public string FluctuationTypeString { get; set; }
    }
}
