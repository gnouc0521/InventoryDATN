using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.InventoryTickets.Dto
{
    [AutoMap(typeof(InventoryTicket))]
    public class InventoryTicketListDto : FullAuditedEntity
    {
        public string Code { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime CompleteTime { get; set; }

        public long WarehouseId { get; set; }
        public long ItemId { get; set; }

        public string CreatedBy { get; set; }

    }
}
