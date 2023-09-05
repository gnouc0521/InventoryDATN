using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Inventorys.Dto
{
    [AutoMap(typeof(Inventory))]
    public class InventoryCreate
    {
        public long? Id { get; set; }   
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public long Quantity { get; set; }
    }
}
