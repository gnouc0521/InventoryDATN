using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.AssessedByYears.Dtos
{
    public class AssessFilter
    {
        public int PersonId { get; set; }

        public AssessedByYearEnum Type { get; set; }
    }

    public class GetAssessedByYearDto : EntityDto
    {
        [Required]
        public int PersonId { get; set; }
        public string Year { get; set; }
        [Required]
        public string SelfAssessment { get; set; }
        [Required]
        [StringLength(255)]
        public string AssessmentByLeader { get; set; }
        [Required]
        [StringLength(255)]
        public string CollectiveFeedback { get; set; }
        [Required]
        [StringLength(128)]
        public string EvaluationOfAuthorizedPerson { get; set; }
        [Required]
        public string ResultsOfClassification { get; set; }

        public string AssessedByYearType { get; set; }
    }
}
