using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto
{
    [AutoMap(typeof(OrderDetail))]
    public class OrdersDetailListDto : FullAuditedEntityDto<long>
    {
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }

        // Cường

        public string ItemName {get; set; }

        public string Specifications {get; set; }





        /// <summary>
        /// kien them
        /// </summary>
        public string Note { get; set; }
        public decimal OrderPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderCode { get; set; }
        public long UserId { get; set; }
        public string ContractCode { get; set; }
        public int  QuantityHT { get; set; }
        public string SupplierName { get; set; }
        public string Itemcode { get; set; }
        public DateTime? DateTimeNeed { get; set; }
        public long SupplierId { get; set; }
        public string CreatedBy { get; set; }
        public bool StatusCreateYCNK { get; set; }
        public long purAssigmentId { get; set; }
        public PurchasesRequestEnum.OrderStatus OrderStatus { get; set; }
        public PurchasesRequestEnum.MyworkStatus WorkStatus { get; set; }
    }
}
