using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.PurchaseAssignments.Dto
{
    [AutoMapTo(typeof(PurchaseAssignment))]
    public class PurchaseAssignmentCreateDto
    {
        public long PurchasesSynthesiseId { get; set; }

        public long UserId { get; set; }

        public long ItemId { get; set; }
        public int Price { get; set; }
        public PurchasesRequestEnum.MyworkStatus Status { get; set; }
        public PurchasesRequestEnum.GetPriceSupkStatus GetPriceStatus { get; set; }

        public int ActualAmount { get; set; }

        public string Link { get; set; }
    }
}
