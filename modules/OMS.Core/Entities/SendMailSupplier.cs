using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppSendMailSupplier", Schema = netcoreConsts.SchemaName)]
    public class SendMailSupplier : FullAuditedEntity
    {
        public long SupplierId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileUrl { get; set; }

        public string Comment { get; set; }
    }
}
