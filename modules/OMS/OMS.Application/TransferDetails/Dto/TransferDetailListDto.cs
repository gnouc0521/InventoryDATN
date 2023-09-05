using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.TransferDetails.Dto
{
    [AutoMap(typeof(TransferDetail))]
    public class TransferDetailListDto : FullAuditedEntityDto<long>
    {
        public long TransferId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public decimal QuotePrice { get; set; }
        public int QuantityInStock { get; set; }
        public int QuantityTransfer { get; set; }
        public string UnitName { get; set; }
        public int IdWarehouseReceiving { get; set; }
        public string WarehouseReceivingName { get; set; }
        public string RequestUnit { get; set; }
        public int IdUnit { get; set; }
    }
}
