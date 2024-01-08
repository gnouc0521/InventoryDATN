using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppStatistical", Schema = netcoreConsts.SchemaName)]
    public class Statistical : Entity
    {
     public long  ItemsId { get; set; }
        public long  WarehouseId { get; set; }
        public int Quantity   { get; set; }
        public DateTime DateStatistical { get; set; }
    }
}
