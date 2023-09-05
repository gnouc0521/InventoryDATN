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

namespace bbk.netcore.mdl.PersonalProfile.Application.GoAbroads.Dto
{

    [AutoMap(typeof(GoAbroad))]
    public class GoAbroadDto : EntityDto<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public long DocumentId { get; set; }

        [Ignore]
        public string DecisionNumber { get; set; }

        [Ignore]
        public DateTime IssuedDate { get; set; }

        [Required]
        public int FromDate { get; set; }

        [Required]
        public int ToDate { get; set; }

        [Required]
        public string Location { get; set; }

        public string Summary { get; set; }
    }
}
