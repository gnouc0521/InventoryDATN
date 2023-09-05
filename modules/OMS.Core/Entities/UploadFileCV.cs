using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppUploadFilesCV", Schema = netcoreConsts.SchemaName)]
    public class UploadFileCV : FullAuditedEntity
    {
        public long? WorkId { get; set; }
        public virtual Work Work { get; set; }
        public string FileName { get; set; }
       
        public string FilePath { get; set; }
        
       
        public string FileUrl { get; set; }



    }
}
