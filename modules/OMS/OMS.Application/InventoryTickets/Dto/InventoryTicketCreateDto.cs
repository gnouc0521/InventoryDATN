using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.InventoryTickets.Dto
{
    [AutoMapTo(typeof(InventoryTicket))]
    public class InventoryTicketCreateDto
    {
        public string Code { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime CompleteTime { get; set; }

        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
    }
}
