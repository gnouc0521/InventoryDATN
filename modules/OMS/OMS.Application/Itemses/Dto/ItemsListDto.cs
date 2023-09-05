using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Itemses.Dto
{
    [AutoMap(typeof(Items))]
    public class ItemsListDto : FullAuditedEntityDto<long>
    {
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProducerCode { get; set; }
        public string SymbolCode { get; set; }
        public string Image { get; set; }
        public string CategoryCode { get; set; }
        public string GroupCode { get; set; }
        public string KindCode { get; set; }
        public string Unit { get; set; }
        public decimal EntryPrice { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public bool Stauts { get; set; }
        public long Quantity { get; set; }
        public int MFG { get; set; }
        public DateTime EXP { get; set; }
        public long InventoryId { get; set; }
        public long WarehouseId { get; set; }

    }
}
