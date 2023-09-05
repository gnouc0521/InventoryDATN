using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.AssessedByYears.Dtos
{
    [AutoMap(typeof(AssessedByYear))]
    public class AssessedByYearDto : Entity
    {
        [Required]
        public int PersonId { get; set; }
        public string Year { get; set; }
        [Required]
        public int SelfAssessmentId { get; set; }
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
        public int ResultsOfClassificationId { get; set; }

        [Required]
        public AssessedByYearEnum AssessedByYearType { get; set; }
    }
      
}
