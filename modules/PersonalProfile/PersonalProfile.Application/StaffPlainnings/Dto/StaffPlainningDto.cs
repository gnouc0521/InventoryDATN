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

namespace bbk.netcore.mdl.PersonalProfile.Application.StaffPlainnings.Dto
{

    [AutoMap(typeof(StaffPlainning))]
    public class StaffPlainningDto : AuditedEntityDto<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required(ErrorMessage ="Hãy chọn quyết đinh!")]
        public long? DocumentId { get; set; }

        [Ignore]
        public string DecisionNumber { get; set; }

        [Ignore]
        public DateTime IssuedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string IssuedLevels { get; set; }
        [Required]
        [StringLength(100)]
        public string WorkingTitle { get; set; }

        public string WorkingTitle1 { get; set; }

        public string WorkingTitle2 { get; set; }

        [Required]
        public int FromDate { get; set; }

        [Required]
        public int ToDate { get; set; }

        public ProfileStaff Person { get; set; }
    }
}
