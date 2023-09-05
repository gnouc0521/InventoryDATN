﻿using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppAssessedByYear", Schema = netcoreConsts.SchemaName)]
    public class AssessedByYear : Entity
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
        public string CollectiveFeedback  { get; set; }
        [Required]
        [StringLength(128)]
        public string EvaluationOfAuthorizedPerson  { get; set; }

        [Required]
        public AssessedByYearEnum AssessedByYearType { get; set; }

        [Required]
        public int ResultsOfClassificationId { get; set; }
        [Required]
        [ForeignKey(nameof(PersonId))]
        public ProfileStaff ProfileStaff { get; set; }
        [ForeignKey(nameof(ResultsOfClassificationId))]
        public Category ResultsOfClassification { get; set; }
        [ForeignKey(nameof(SelfAssessmentId))]
        public Category SelfAssessment { get; set; }
    }
}

