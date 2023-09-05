using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppUnit", Schema = netcoreConsts.SchemaName)]
    public class Unit : Entity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParrentId { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

    }
}
