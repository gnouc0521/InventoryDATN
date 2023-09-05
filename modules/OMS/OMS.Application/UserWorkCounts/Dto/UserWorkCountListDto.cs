﻿using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.UserWorkCounts.Dto
{
    [AutoMap(typeof(UserWorkCount))]
    public class UserWorkCountListDto : AuditedEntity
    {
        public long UserId { get; set; }
        public int PurchaseAssignmentId { get; set; }
        public long PurchasesRequestId { get; set; }
        public long PurchasesSynthesisesId { get; set; }

        public DashboardEnum.TypeStatus TypeStatus { get; set; }
        public PurchasesRequestEnum.MyworkStatus WorkStatus { get; set; }
        public PurchasesRequestEnum.OwnerStatusEnum OwnerStatus { get; set; }
    }
}
