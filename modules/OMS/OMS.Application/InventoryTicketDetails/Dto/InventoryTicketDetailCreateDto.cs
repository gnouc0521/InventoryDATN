using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.InventoryTicketDetails.Dto
{
    [AutoMapTo(typeof(InventoryTicketDetail))]
    public class InventoryTicketDetailCreateDto
    {
        public long InventoryTicketsId { get; set; }
        public virtual InventoryTicket InventoryTicket { get; set; }

        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public int UnitName { get; set; }
        public DateTime ExpireDate { get; set; }
        public long ParcelId { get; set; }
       
        public string ParcelCode { get; set; }
      
        public string Remark { get; set; }

        public long WarehouseSourceId { get; set; }
        public long BlockId { get; set; }

        public long FloorId { get; set; }

        public long ShelfId { get; set; }
    }
}
