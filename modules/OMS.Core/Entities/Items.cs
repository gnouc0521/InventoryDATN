using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppItems", Schema = netcoreConsts.SchemaName)]
    public class Items : FullAuditedEntity<long>
    {
        [StringLength(50)]
        public string ItemCode { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(50)]
        public string ProducerCode { get; set; }
        [StringLength(50)]
        public string SymbolCode { get; set; }
        [StringLength(500)]
        public string Image { get; set; }
        [StringLength(10)]
        public string CategoryCode { get; set; }
        [StringLength(10)]
        public string GroupCode { get; set; }
        [StringLength(10)]
        public string KindCode { get; set; }
        public ICollection<Assignment> Assignments { get; set; }

    }
}
