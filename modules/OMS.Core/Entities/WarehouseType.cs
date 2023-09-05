using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppWarehouseType", Schema = netcoreConsts.SchemaName)]
    public class WarehouseType :Entity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }
    }
}
