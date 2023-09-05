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

namespace bbk.netcore.mdl.PersonalProfile.Application.TrainningInfos.Dto
{
    [AutoMap(typeof(TrainningInfo))]
    public class TrainningInfoDto : AuditedEntityDto<int>
    {
        [Required]
        public int PersonId { get; set; }

        public int TrainningTypeId { get; set; }

        [Required]
        public int DiplomaId { get; set; }

        [Required]
        [StringLength(100)]
        public string SchoolName { get; set; }

        [Required]
        [StringLength(100)]
        public string MajoringName { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public string FileUrl { get; set; }

        public string FilePath { get; set; }
    }
}
