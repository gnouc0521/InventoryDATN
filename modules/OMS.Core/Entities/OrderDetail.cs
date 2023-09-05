using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppOrderDetail", Schema = netcoreConsts.SchemaName)]
    public class OrderDetail : FullAuditedEntity<long>
    {
        public long  ItemId { get; set; }
        public long  OrderId { get; set; }
        public int Quantity { get; set; }

        public int UnitId { get; set; } 
        public string UnitName { get; set; } 

        public string Note { get; set; }
        public decimal OrderPrice { get; set; }
        public string Specifications { get; set; }




    }
}
