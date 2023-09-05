using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.InventoryTicketDetails.Dto
{
    [AutoMap(typeof(InventoryTicketDetail))]
    public class InventoryTicketDetailListDto:FullAuditedEntity
    {
        public long InventoryTicketsId { get; set; }
        public virtual InventoryTicket InventoryTicket { get; set; }

        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public DateTime ExpireDate { get; set; }
        public long ParcelId { get; set; }

        public string ParcelCode { get; set; }

        public string Remark { get; set; }

        public long WarehouseSourceId { get; set; }
        public long BlockId { get; set; }

        public long FloorId { get; set; }

        public long ShelfId { get; set; }

        public string CodeItem { get; set; }

        public string NameItem { get; set; }

        public decimal Dongia { get; set; }

        public long SoluongHT { get; set; }

        public int stt { get; set; }

        public string NameCode { get; set; }

        public long SoLuongTru { get; set; }

        public long QuantityIN { get; set; }
        public long QuantityOUT { get; set; }

    }
}
