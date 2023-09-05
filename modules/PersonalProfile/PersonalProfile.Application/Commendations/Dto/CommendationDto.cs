using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using AutoMapper.Configuration.Annotations;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Commendations.Dto
{
    [AutoMap(typeof(Commendation))]
    public class CommendationDto : AuditedEntityDto<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Hãy chọn quyết đinh!")]
        public long? DocumentId { get; set; }

        [Ignore]
        public string DecisionNumber { get; set; }

        [Required]
        public int CommendationYear { get; set; }

        [Required]
        public int CommendationTitleId { get; set; }

        public ProfileStaff ProfileStaff { get; set; }

        public Category CommendationTitle { get; set; }
    }
}
