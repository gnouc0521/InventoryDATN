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
    public class GetAllFilter
    {
        public int PersonId { get; set; }
    }

    public class GetTrainningInfoDto : EntityDto<int>
    {
        public int PersonId { get; set; }

        public string TrainningType { get; set; }

        public string Diploma { get; set; }
        public string FileUrl { get; set; }
        public string FilePath { get; set; }

        public string SchoolName { get; set; }

        public string MajoringName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
