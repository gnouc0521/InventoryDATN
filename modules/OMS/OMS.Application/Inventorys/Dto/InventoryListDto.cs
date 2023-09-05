using Abp.Application.Services.Dto;
using AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Inventorys.Dto
{
    [AutoMap(typeof(Inventory))]
    public class InventoryListDto : FullAuditedEntityDto<long>
    {
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public long Quantity { get; set; }

        public int stt { get; set; }
        public string NameCode { get; set; }

        public decimal ImportPrice { get; set; }

        public string NameItem { get; set; }

        public string CodeItem { get; set; }
        public string WarehouseName { get; set; }

        public long QuantityIN { get; set; }
        public int QuantityOUT { get; set; }

    }
}
