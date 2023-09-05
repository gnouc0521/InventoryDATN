using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.TrainningInfos.Dto
{
    public class UploadFileDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FileUrl{ get; set; }

        [Required]
        public string FilePath{ get; set; }
    }
}
