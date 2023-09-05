using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.PurchaseAssignments.Dto
{
    [AutoMap(typeof(PurchaseAssignment))]
    public class PurchaseAssigmentListDto :FullAuditedEntity
    {
        public long PurchasesSynthesiseId { get; set; }

        public long UserId { get; set; }

        public long ItemId { get; set; }

        public PurchasesRequestEnum.GetPriceSupkStatus GetPriceStatus { get; set; }
        public PurchasesRequestEnum.MyworkStatus Status { get; set; }

        public int ActualAmount { get; set; }

        public int Price { get; set; }
    }
}
