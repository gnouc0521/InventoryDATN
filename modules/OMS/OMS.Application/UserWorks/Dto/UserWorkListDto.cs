using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.UserWorks.Dto
{
    [AutoMap(typeof(UserWork))]
    public class UserWorkListDto
    {
        public long UserId { get; set; }
        public int WorkId { get; set; }
        public WorkEnum.Status Status { get; set; }
        public WorkEnum.OwnerStatusEnum OwnerStatus { get; set; }

    }
}
