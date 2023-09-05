using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppRules", Schema = netcoreConsts.SchemaName)]
    public class Rules : Entity
    {
     public string  ItemKey { get; set; }
        public int  Order { get; set; }
        public string  ItemText { get; set; }
        public string  ItemValue { get; set; }
        public string  Culture { get; set; }
        public string  Remark { get; set; }
    }
}
