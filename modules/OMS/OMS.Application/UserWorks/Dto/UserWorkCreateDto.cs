using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto;
using bbk.netcore.mdl.OMS.Application.WorkGroups.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.UserWorks.Dto
{
    [AutoMapTo(typeof(UserWork))]
    public class UserWorkCreateDto
    {
        public long UserId { get; set; }
        public int WorkId { get; set; }
        public WorkEnum.Status Status { get; set; }
        public WorkEnum.OwnerStatusEnum OwnerStatus { get; set; }
    }
}
