using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Orders.Dto
{
    [AutoMap(typeof(Order))]
    public class OrderListDto : FullAuditedEntityDto<long>
    {
        public string OrderCode { get; set; }
        public PurchasesRequestEnum.OrderStatus OrderStatus { get; set; }
        public long ContractId { get; set; }
        public string ContractCode { get; set; }
        public long UserId { get; set; }
        public long QuoteId { get; set; }
        public string ExpertName { get; set; }
        public long OrderId { get; set; }
        public bool StatusCreateYCNK { get; set; }
        public string SupplierName { get; set; }


    }
}
